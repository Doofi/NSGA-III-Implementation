using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.RealVectorEncoding;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;

namespace HeuristicLab.Algorithms.NSGA3
{
    /// <summary>
    /// The Nondominated Sorting Genetic Algorithm III was introduced in Jain, Deb 2013: An
    /// Evolutionary Many-Objective Optimization Algorithm Using Reference - Point - Based
    /// Nondominated Sorting Approach, Part II: Handling Constraints and Extending to an Adaptive
    /// Approach. IEEE Transactions on Evolutionary Computation (Volume: 18, Issue: 4, Aug, 2014)
    /// pp. 602-622.
    /// </summary>
    [Item("NSGA-III", "The Nondominated Sorting Genetic Algorithm III was introduced in Jain, Deb 2013: An Evolutionary Many-Objective Optimization Algorithm Using Reference - Point - Based Nondominated Sorting Approach, Part II: Handling Constraints and Extending to an Adaptive Approach. IEEE Transactions on Evolutionary Computation (Volume: 18, Issue: 4, Aug, 2014) pp. 602-622.")]
    [Creatable(CreatableAttribute.Categories.PopulationBasedAlgorithms, Priority = 136)]
    [StorableType("ce2961e8-0cd9-4dc8-b441-d9059a690874")]
    internal class NSGA3 : BasicAlgorithm
    {
        // Todo: get methods in order

        public override bool SupportsPause => false; // todo: make true

        #region ProblemProperties

        public override Type ProblemType
        {
            get { return typeof(MultiObjectiveBasicProblem<RealVectorEncoding>); }
        }

        public new MultiObjectiveBasicProblem<RealVectorEncoding> Problem
        {
            get { return (MultiObjectiveBasicProblem<RealVectorEncoding>)base.Problem; }
            set { base.Problem = value; }
        }

        #endregion ProblemProperties

        #region Storable fields

        #endregion Storable fields

        #region ParameterNames

        #endregion ParameterNames

        #region ParameterProperties

        private ValueParameter<IntValue> SeedParameter
        {
            get { return (ValueParameter<IntValue>)Parameters["Seed"]; }
        }

        private ValueParameter<BoolValue> SetSeedRandomlyParameter
        {
            get { return (ValueParameter<BoolValue>)Parameters["SetSeedRandomly"]; }
        }

        private ValueParameter<IntValue> PopulationSizeParameter
        {
            get { return (ValueParameter<IntValue>)Parameters["PopulationSize"]; }
        }

        private ValueParameter<PercentValue> CrossoverProbabilityParameter
        {
            get { return (ValueParameter<PercentValue>)Parameters["CrossoverProbability"]; }
        }

        private ValueParameter<PercentValue> MutationProbabilityParameter
        {
            get { return (ValueParameter<PercentValue>)Parameters["MutationProbability"]; }
        }

        //private ValueParameter<MultiAnalyzer> AnalyzerParameter
        //{
        //    get { return (ValueParameter<MultiAnalyzer>)Parameters["Analyzer"]; }
        //}

        private ValueParameter<IntValue> MaximumGenerationsParameter
        {
            get { return (ValueParameter<IntValue>)Parameters["MaximumGenerations"]; }
        }

        private IFixedValueParameter<BoolValue> DominateOnEqualQualitiesParameter
        {
            get { return (IFixedValueParameter<BoolValue>)Parameters["DominateOnEqualQualities"]; }
        }

        #endregion ParameterProperties

        #region Properties

        public IntValue Seed
        {
            get { return SeedParameter.Value; }
            set { SeedParameter.Value = value; }
        }

        public BoolValue SetSeedRandomly
        {
            get { return SetSeedRandomlyParameter.Value; }
            set { SetSeedRandomlyParameter.Value = value; }
        }

        public IntValue PopulationSize
        {
            get { return PopulationSizeParameter.Value; }
            set { PopulationSizeParameter.Value = value; }
        }

        public PercentValue CrossoverProbability
        {
            get { return CrossoverProbabilityParameter.Value; }
            set { CrossoverProbabilityParameter.Value = value; }
        }

        public PercentValue MutationProbability
        {
            get { return MutationProbabilityParameter.Value; }
            set { MutationProbabilityParameter.Value = value; }
        }

        //public MultiAnalyzer Analyzer
        //{
        //    get { return AnalyzerParameter.Value; }
        //    set { AnalyzerParameter.Value = value; }
        //}

        public IntValue MaximumGenerations
        {
            get { return MaximumGenerationsParameter.Value; }
            set { MaximumGenerationsParameter.Value = value; }
        }

        public bool DominateOnEqualQualities
        {
            get { return DominateOnEqualQualitiesParameter.Value.Value; }
            set { DominateOnEqualQualitiesParameter.Value.Value = value; }
        }

        #endregion Properties

        #region ResultsProperties

        #endregion ResultsProperties

        #region Constructors

        // Called when creating new NSGA3 instance
        public NSGA3() : base()
        {
            Parameters.Add(new ValueParameter<IntValue>("Seed", "The random seed used to initialize the new pseudo random number generator.", new IntValue(0)));
            Parameters.Add(new ValueParameter<BoolValue>("SetSeedRandomly", "True if the random seed should be set to a random value, otherwise false.", new BoolValue(true)));
            Parameters.Add(new ValueParameter<IntValue>("PopulationSize", "The size of the population of solutions.", new IntValue(100)));
            Parameters.Add(new ValueParameter<PercentValue>("CrossoverProbability", "The probability that the crossover operator is applied on two parents.", new PercentValue(0.9)));
            Parameters.Add(new ValueParameter<PercentValue>("MutationProbability", "The probability that the mutation operator is applied on a solution.", new PercentValue(0.05)));
            //Parameters.Add(new ValueParameter<MultiAnalyzer>("Analyzer", "The operator used to analyze each generation.", new MultiAnalyzer()));
            Parameters.Add(new ValueParameter<IntValue>("MaximumGenerations", "The maximum number of generations which should be processed.", new IntValue(1000)));
            Parameters.Add(new FixedValueParameter<BoolValue>("DominateOnEqualQualities", "Flag which determines wether solutions with equal quality values should be treated as dominated.", new BoolValue(false)));
        }

        protected NSGA3(NSGA3 original, Cloner cloner) : base(original, cloner)
        {
            // Todo: clone Storable fields
        }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new NSGA3(this, cloner);
        }

        #endregion Constructors

        #region Initialization

        #endregion Initialization

        protected override void Run(CancellationToken cancellationToken)
        {
            while (true)
            {
                Debug.WriteLine("Hello World! from Run");

                Thread.Sleep(100);
            }
        }

        private const int dimensionCount = 10; // todo: use parameters

        /// <summary>
        /// returns population of next generation after applying necessary operations
        /// </summary>
        /// <param name="pt">parent population</param>
        /// <param name="referencePoints">structured reference points or supplied aspiration points</param>
        /// <returns></returns>
        private List<Solution> GetNextGeneration(List<Solution> pt, List<ReferencePoint> referencePoints)
        {
            bool aspirationPointsWereSupplied = false; // todo: use parameters

            List<Solution> st = new List<Solution>();
            int i = 1;

            Recombine(pt);
            Mutate(pt);
            double[][] qualities = Evaluate(pt);
            bool[] maximization = new bool[dimensionCount];
            int[] rank;

            List<List<Tuple<Solution, double[]>>> allParetoFronts = DominationCalculator<Solution>.CalculateAllParetoFronts(pt.ToArray(), qualities, maximization, out rank, true);

            return null;
        }

        private void Recombine(List<Solution> pt)
        {
            throw new NotImplementedException();
        }

        private void Mutate(List<Solution> pt)
        {
            throw new NotImplementedException();
        }

        private double[][] Evaluate(List<Solution> pt)
        {
            throw new NotImplementedException();
        }

        // Called when Algorithm is starting to execute. todo: remove this comment
        protected override void Initialize(CancellationToken cancellationToken)
        {
            int paretoSolutionCount = 20; // todo: remove

            // Todo: Initialize results screen
            Results.Add(new Result("Iterations", new IntValue(0)));
            Results.Add(new Result("Best solutions", new IntMatrix(dimensionCount, paretoSolutionCount)));

            base.Initialize(cancellationToken);
        }
    }
}