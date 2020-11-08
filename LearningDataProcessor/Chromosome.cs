using Assets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningDataProcessor
{
    public class Chromosome
    {
        public List<Gene> Genes { get; set; } // TODO array daca stii lungimea de la inceput

        public double Fitness { get; set; }

        public double CumulativeFitness { get; set; }

        public double FunctionValue { get; set; }

        public double RealFunctionValue { get; set; }

        public double AssociativeValue { get; set; }

        public double thetha0 { get; set; }

        public double thetha1 { get; set; }

        public double thetha2 { get; set; }

        public Chromosome()
        {

        }

        public Chromosome(int length)
        {
            Genes = new List<Gene>(length);

            for (int i = 0; i < Genes.Capacity; i++)
            {
                Gene g = new Gene(0);
                Genes.Add(g);
            }

            CumulativeFitness = 0.0;
        }

        public Chromosome(int length, Random random)
        {
            Genes = new List<Gene>(length);

            for (int i = 0; i < Genes.Capacity; i++)
            {
                int randomNumber = random.Next(0, 2);

                Gene g = new Gene(randomNumber);
                Genes.Add(g);
            }

            CumulativeFitness = 0.0;
        }

        public double GetBase10()
        {
            return Convert.ToInt32(this.ToString(), 2);
        }

        public Tuple<double, double, double> GetCoefficients()
        {
            string chromosome = this.ToString();

            this.thetha0 = Convert.ToInt32(chromosome.Substring(0, 8), 2);
            this.thetha1 = Convert.ToInt32(chromosome.Substring(8, 8), 2);
            this.thetha2 = Convert.ToInt32(chromosome.Substring(16, 8), 2);

            return new Tuple<double, double, double>(thetha0, thetha1, thetha2);
        }

        public override string ToString()
        {
            string binaryRepresentation = string.Empty;

            foreach (Gene g in Genes)
            {
                binaryRepresentation = string.Format("{0}{1}", binaryRepresentation, g.Value);
            }

            return binaryRepresentation;
        }
    }
}
