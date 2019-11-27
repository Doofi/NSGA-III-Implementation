using HEAL.Attic;
using HeuristicLab.Common; // required for parameters collection
using HeuristicLab.Core; // required for parameters collection
using HeuristicLab.Data; // IntValue, ...
using HeuristicLab.Encodings.BinaryVectorEncoding;
using HeuristicLab.Optimization; // BasicAlgorithm
using HeuristicLab.Parameters;
using HeuristicLab.Problems.Binary;
using HeuristicLab.Random; // MersenneTwister
using System;
using System.Threading;

namespace HeuristicLab.Algorithms.NSGA3
{
    // each HL item needs to have a name and a description (BasicAlgorithm is an Item) The name and
    // description of items is shown in the GUI
    [Item("NSGA-III", "The Nondominated Sorting Genetic Algorithm III was introduced in Jain, Deb 2013: An Evolutionary Many-Objective Optimization Algorithm Using Reference - Point - Based Nondominated Sorting Approach, Part II: Handling Constraints and Extending to an Adaptive Approach. IEEE Transactions on Evolutionary Computation (Volume: 18, Issue: 4, Aug, 2014) pp. 602-622.")]

    // If the algorithm should be shown in the "New..." dialog it must be creatable. Entries in the
    // new dialog are grouped to categories and ordered by priorities
    [Creatable(Category = CreatableAttribute.Categories.Algorithms, Priority = 999)]
    [StorableType("689280F7-E371-44A2-98A5-FCEDF22CA343")] // for persistence (storing your algorithm to a files or transfer to HeuristicLab.Hive
    public class NSGA3 : BasicAlgorithm
    {
        // createable items must have a default ctor
        public NSGA3()
        {
            // algorithm parameters are shown in the GUI
            Parameters.Add(new FixedValueParameter<IntValue>("MaxIterations", new IntValue(10000)));
        }

        // Persistence uses this ctor to improve deserialization efficiency. If we would use the
        // default ctor instead this would completely initialize the object (e.g. creating
        // parameters) even though the data is later overwritten by the stored data.
        [StorableConstructor]
        public NSGA3(StorableConstructorFlag _) : base(_) { }

        // Each clonable item must have a cloning ctor (deep cloning, the cloner is used to handle
        // cyclic object references)
        public NSGA3(NSGA3 original, Cloner cloner) : base(original, cloner)
        {
            // Don't forget to call the cloning ctor of the base class This class does not have
            // fields, therefore we don't need to actually clone anything
        }

        // This algorithm only works for BinaryProblems. Overriding the ProblemType property has the
        // effect that only BinaryProblems can be set as problem for the algorithm in the GUI
        public override Type ProblemType { get { return typeof(BinaryProblem); } }

        public new BinaryProblem Problem { get { return (BinaryProblem)base.Problem; } }

        #region parameters

        public int MaxIterations
        {
            get { return MaxIterationsParameter.Value.Value; }
            set { MaxIterationsParameter.Value.Value = value; }
        }

        // If an algorithm has parameters then we usually also add properties to access these
        // parameters. This is not strictly required but considered good shape.
        private IFixedValueParameter<IntValue> MaxIterationsParameter
        {
            get { return (IFixedValueParameter<IntValue>)Parameters["MaxIterations"]; }
        }

        #endregion parameters

        public override bool SupportsPause
        {
            get { return false; }
        }

        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new NSGA3(this, cloner);
        }

        protected override void Run(CancellationToken cancellationToken)
        {
            int maxIters = MaxIterations;
            var problem = Problem;
            var rand = new MersenneTwister(1234);

            var bestQuality = problem.Maximization ? double.MinValue : double.MaxValue;

            var curItersItem = new IntValue();
            var bestQualityItem = new DoubleValue(bestQuality);
            var curItersResult = new Result("Iteration", curItersItem);
            var bestQualityResult = new Result("Best quality", bestQualityItem);
            Results.Add(curItersResult);
            Results.Add(bestQualityResult);

            for (int i = 0; i < maxIters; i++)
            {
                curItersItem.Value = i;

                // ----------------------------- IMPLEMENT YOUR ALGORITHM HERE -----------------------------

                // this is an example for random search for a more elaborate algorithm check the
                // source code of "HeuristicLab.Algorithms.ParameterlessPopulationPyramid"
                var cand = new BinaryVector(problem.Length, rand);
                var quality = problem.Evaluate(cand, rand); // calling Evaluate like this is not possible for all problems...
                if (problem.Maximization) bestQuality = Math.Max(bestQuality, quality);
                else bestQuality = Math.Min(quality, bestQuality);
                bestQualityItem.Value = bestQuality;

                // check the cancellation token to see if the used clicked "Stop"
                if (cancellationToken.IsCancellationRequested) break;
            }

            Results.Add(new Result("Execution time", new TimeSpanValue(this.ExecutionTime)));
        }
    }
}