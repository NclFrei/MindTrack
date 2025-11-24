using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.DTOs.Response;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.ML;
using MindTrack.Domain.Models;

namespace MindTrack.Application.Service.V2
{
    public class TaskOrganizerService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IStressRepository _stressRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskOrganizerService> _logger;

        private readonly MLContext _ml;
        private readonly PredictionEngine<TaskRankingInput, TaskRankingOutput> _engine;

        public TaskOrganizerService(
            ITarefaRepository tarefaRepository,
            IStressRepository stressRepository,
            IMapper mapper,
            ILogger<TaskOrganizerService>? logger = null)
        {
            _tarefaRepository = tarefaRepository;
            _stressRepository = stressRepository;
            _mapper = mapper;
            _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<TaskOrganizerService>.Instance;

            // Inicializa ML.NET
            _ml = new MLContext();

            // -------------------------------
            //  TREINAMENTO DO MODELO OTIMIZADO
            // -------------------------------

            // pipeline com FastTree (rápido e estável)
            var pipeline = _ml.Transforms.Concatenate(
                    "Features",
                    nameof(TaskRankingInput.Prioridade),
                    nameof(TaskRankingInput.Dificuldade),
                    nameof(TaskRankingInput.MetaId),
                    nameof(TaskRankingInput.StressScore)
                )
                .Append(_ml.Regression.Trainers.FastTree(
                    labelColumnName: "Label",
                    featureColumnName: "Features"
                ));

            // Dados de treino sintéticos (pode melhorar depois)
            var trainingData = new List<TaskRankingInput>
            {
                // Stress alto = priorizar coisas fáceis e urgentes
                new() { Prioridade = 3, Dificuldade = 0, MetaId = 0, StressScore = 90, Label = 1.0f },
                new() { Prioridade = 3, Dificuldade = 1, MetaId = 0, StressScore = 90, Label = 0.9f },
                new() { Prioridade = 2, Dificuldade = 0, MetaId = 0, StressScore = 90, Label = 0.8f },
                new() { Prioridade = 1, Dificuldade = 3, MetaId = 0, StressScore = 90, Label = 0.2f },

                // Stress moderado
                new() { Prioridade = 3, Dificuldade = 2, MetaId = 1, StressScore = 50, Label = 0.7f },
                new() { Prioridade = 2, Dificuldade = 2, MetaId = 1, StressScore = 50, Label = 0.5f },

                // Stress baixo = pode encarar tarefas difíceis
                new() { Prioridade = 1, Dificuldade = 3, MetaId = 0, StressScore = 10, Label = 0.7f },
                new() { Prioridade = 2, Dificuldade = 3, MetaId = 0, StressScore = 10, Label = 0.8f },
                new() { Prioridade = 3, Dificuldade = 3, MetaId = 0, StressScore = 10, Label = 1.0f }
            };


            var trainDataView = _ml.Data.LoadFromEnumerable(trainingData);

            // Treinar modelo
            var model = pipeline.Fit(trainDataView);

            // PredictionEngine
            _engine = _ml.Model.CreatePredictionEngine<TaskRankingInput, TaskRankingOutput>(model);
        }

        public async Task<List<TarefaResponse>> OrganizeAsync(OrganizeTasksRequest request)
        {
            _logger.LogInformation("Organizando tarefas do usuário {UserId}", request.UserId);

            // 1️⃣ Buscar tarefas
            var tarefas = await _tarefaRepository.GetByUserIdAsync(request.UserId);
            if (tarefas == null || tarefas.Count == 0)
                return new List<TarefaResponse>();

            // 2️⃣ Último StressScore
            var lastStress = await _stressRepository.GetLatestByUserIdAsync(request.UserId);
            var stressValue = lastStress?.Score ?? 30;

            // 3️⃣ Ranquear via ML.NET
            var ranked = tarefas
                .Select(t =>
                {
                    var input = new TaskRankingInput
                    {
                        Prioridade = t.Prioridade,
                        Dificuldade = (float)t.Dificuldade,   // enum convertido
                        MetaId = t.MetaId ?? 0,
                        StressScore = stressValue,
                        Label = 0 // ignorado na previsão
                    };

                    var prediction = _engine.Predict(input);

                    return new
                    {
                        Task = t,
                        Ranking = prediction.Score
                    };
                })
                .OrderByDescending(x => x.Ranking)
                .Select(x => _mapper.Map<TarefaResponse>(x.Task))
                .ToList();

            return ranked;
        }
    }
}
