using Assets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Input;

namespace LearningDataProcessor
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        #region fields

        private double a;

        private double b;

        private int population;

        private int chromosomeLength;

        private double meanValue;

        private ICommand _nextGeneration;

        private ICommand _generateInit;

        private ICommand _getCoefficients;

        private bool _canExecute;

        private int generationNumber;

        private readonly Random random = new Random();

        #endregion

        #region properties

        public double A
        {
            get { return this.a; }
            set
            {
                this.a = value;
                OnPropertyChanged("A");
            }
        }

        public double B
        {
            get { return this.b; }
            set
            {
                this.b = value;
                OnPropertyChanged("B");
            }
        }

        public int PopulationCount
        {
            get { return this.population; }
            set
            {
                this.population = value;
                OnPropertyChanged("Population");
            }
        }

        public int ChromosomeLength
        {
            get { return this.chromosomeLength; }
            set
            {
                this.chromosomeLength = value;
                OnPropertyChanged("ChromosomeLength");
            }
        }

        public double MeanValue
        {
            get { return this.meanValue; }
            set
            {
                this.meanValue = value;
                OnPropertyChanged("MeanValue");
            }
        }

        public ICommand NextGeneration
        {
            get
            {
                return _nextGeneration ?? (_nextGeneration = new CommandHandler(() => NextPopulationGeneration(), _canExecute));
            }
        }

        public ICommand GenerateInit
        {
            get
            {
                return _generateInit ?? (_generateInit = new CommandHandler(() => GenerateInitPopulation(), _canExecute));
            }
        }

        public ICommand GetCoefficients
        {
            get
            {
                return _getCoefficients ?? (_getCoefficients = new CommandHandler(() => GetCurrentFittestCoefficients(), _canExecute));
            }
        }

        private const double CrossoverParamter = 0.1;

        private const double MutationParamter = 0.01;

        public List<Chromosome> CurrentPopulation { get; set; }

        private ObservableCollection<LearningData> learningData;

        public ObservableCollection<LearningData> LearningData
        {
            get { return learningData; }
            set
            {
                learningData = value;
                OnPropertyChanged("LearningData");
            }
        }

        public int GenerationNumber
        {
            get { return this.generationNumber; }
            set
            {
                this.generationNumber = value;
                OnPropertyChanged("GenerationNumber");
            }
        }
        #endregion

        #region constructors

        public MainWindowViewModel()
        {
            // default values
            _canExecute = true;

            A = 0.0;
            B = 50.0;
            PopulationCount = 200;
            ChromosomeLength = 24;
            MeanValue = 0.0;
            GenerationNumber = 0;
        }

        #endregion

        #region methods

        public void SaveData(object objectToSave, string fileName)
        {
            // Add the File Path together with the files name and extension.
            // We will use .bin to represent that this is a Binary file.
            string FullFilePath = @"C:/Users/Vlad/AppData/LocalLow/Awesome Inc U/Pong Game/" + fileName + ".bin";
            // We must create a new Formattwr to Serialize with.
            BinaryFormatter Formatter = new BinaryFormatter();
            // Create a streaming path to our new file location.
            FileStream fileStream = new FileStream(FullFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            // Serialize the objedt to the File Stream
            Formatter.Serialize(fileStream, objectToSave);
            // FInally Close the FileStream and let the rest wrap itself up.
            fileStream.Close();
        }

        public object LoadData(string fileName)
        {
            string FullFilePath = @"C:/Users/Vlad/AppData/LocalLow/Awesome Inc U/Pong Game/" + fileName + ".bin";
            // Check if our file exists, if it does not, just return a null object.
            if (File.Exists(FullFilePath))
            {
                BinaryFormatter Formatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(FullFilePath, FileMode.Open);
                object obj = Formatter.Deserialize(fileStream);
                fileStream.Close();
                // Return the uncast untyped object.
                return obj;
            }
            else
            {
                return null;
            }
        }

        public void GenerateInitPopulation()
        {
            object loadedData = LoadData("PongLearningData");

            if (loadedData != null)
            {
                LearningData = new ObservableCollection<LearningData>(loadedData as List<LearningData>);
            }

            CurrentPopulation = new List<Chromosome>(PopulationCount);
            GenerationNumber = 1;

            int seed = (int)DateTime.Now.Ticks;
            Random random = new Random(seed);

            for (int i = 0; i < PopulationCount; i++)
            {
                Chromosome c = new Chromosome(ChromosomeLength, random);
                CurrentPopulation.Add(c);
            }

            EvaluatePopulation(CurrentPopulation);
            MeanValue = 0.0;
            MeanValue = GetMeanValue(CurrentPopulation);

            if (this.GeneratedNewPopulation != null)
            {
                GeneratedNewPopulation.Invoke(CurrentPopulation, new EventArgs());
            }

            if (this.ResetPopulation != null)
            {
                ResetPopulation.Invoke(CurrentPopulation, new EventArgs());
            }
        }

        public void GetCurrentFittestCoefficients()
        {
            Chromosome bestChromosome = CurrentPopulation.First();

            foreach (Chromosome c in CurrentPopulation)
            {
                if (c.Fitness > bestChromosome.Fitness)
                {
                    bestChromosome = c;
                }
            }
            Tuple<double, double, double> cf = bestChromosome.GetCoefficients();

            List<double> coefficients = new List<double>();
            coefficients.Add(cf.Item1);
            coefficients.Add(cf.Item2);
            coefficients.Add(cf.Item3);

            SaveData(coefficients, "FunctionCoefficients");
        }

        public List<Chromosome> GenerateNewPopulation(List<Chromosome> population)
        {
            List<Chromosome> newPopulation = new List<Chromosome>(PopulationCount);

            for (int i = 0; i < PopulationCount; i++)
            {
                Chromosome c = new Chromosome(ChromosomeLength)
                {
                    Genes = population[i].Genes
                };

                newPopulation.Add(c);
            }

            return newPopulation;
        }

        public void NextPopulationGeneration()
        {
            CurrentPopulation = Selection(CurrentPopulation); // embed pe un tip de date.. populatie

            CurrentPopulation = Crossover(CurrentPopulation);

            CurrentPopulation = Mutation(CurrentPopulation);

            CurrentPopulation = GenerateNewPopulation(CurrentPopulation); // TODO remove

            EvaluatePopulation(CurrentPopulation);
            MeanValue = GetMeanValue(CurrentPopulation);

            if (this.GeneratedNewPopulation != null)
            {
                GeneratedNewPopulation.Invoke(CurrentPopulation, new EventArgs());
            }

            GenerationNumber++;
        }

        /// <summary>
        /// Selection process
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        private List<Chromosome> Selection(List<Chromosome> population)
        {
            List<Chromosome> newPopulation = new List<Chromosome>();

            // selection
            List<double> randomSelectionNumbers = GenerateRandomSelectionNumbers(PopulationCount);

            foreach (double d in randomSelectionNumbers)
            {
                if (d <= population[0].CumulativeFitness)
                {
                    // introducem cromozomul in noua populatie ---> population[0]
                    // clonare population[0] si pentru restul cazurilor TODO
                    newPopulation.Add(population[0]);
                }
                else
                {
                    for (int i = 0; i < PopulationCount - 1; i++)
                    {
                        if (d > population[i].CumulativeFitness && d <= population[i + 1].CumulativeFitness)
                        {
                            // introducem cromozomul in noua populatie ---> population[i+1]
                            // clonare population[0] si pentru restul cazurilor TODO
                            newPopulation.Add(population[i + 1]);
                        }
                    }
                }

            }

            return newPopulation;
        }

        /// <summary>
        /// Crossover process
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        private List<Chromosome> Crossover(List<Chromosome> population)
        {
            // TODO declara capacitatea initiala a listei daca este cunoscuta
            List<Chromosome> newPopulation = new List<Chromosome>();
            List<Chromosome> crossoverPopulation = new List<Chromosome>();

            foreach (Chromosome c in population)
            {
                double randomNumber = random.NextDouble();

                if (randomNumber < CrossoverParamter)
                {
                    // selecteaza cromozom pentru incrucisare
                    crossoverPopulation.Add(c);
                }
                else
                {
                    // adauga la noua populatie
                    newPopulation.Add(c);
                }
            }

            if (crossoverPopulation.Count % 2 != 0)
            {
                newPopulation.Add(crossoverPopulation[crossoverPopulation.Count - 1]);
                crossoverPopulation.RemoveAt(crossoverPopulation.Count - 1);
            }

            for (int i = 0; i < crossoverPopulation.Count - 1; i = i + 2)
            {
                int crossoverNr = random.Next(1, chromosomeLength - 1);

                Chromosome parent1 = crossoverPopulation[i];
                Chromosome parent2 = crossoverPopulation[i + 1];

                Chromosome child1 = new Chromosome(ChromosomeLength);
                Chromosome child2 = new Chromosome(ChromosomeLength);

                int geneNr = 0;

                // mix the parents
                foreach (Gene g in parent1.Genes)
                {
                    if (geneNr < crossoverNr)
                    {
                        child1.Genes[geneNr] = parent1.Genes[geneNr];

                    }
                    else
                    {
                        child1.Genes[geneNr] = parent2.Genes[geneNr];
                    }

                    geneNr++;
                }

                geneNr = 0;

                // TODO check GEnes maybe as byte[]
                foreach (Gene g in parent2.Genes)
                {
                    if (geneNr < crossoverNr)
                    {
                        child2.Genes[geneNr] = parent2.Genes[geneNr];

                    }
                    else
                    {
                        child2.Genes[geneNr] = parent1.Genes[geneNr];
                    }

                    geneNr++;
                }

                newPopulation.Add(child1);
                newPopulation.Add(child2);
            }

            return newPopulation;
        }

        // todo
        private Tuple<Chromosome, Chromosome> CrossoverChromosomes(Chromosome parent1, Chromosome parent2)
        {
            Chromosome child1 = new Chromosome(ChromosomeLength);
            Chromosome child2 = new Chromosome(ChromosomeLength);






            return new Tuple<Chromosome, Chromosome>(child1, child2);
        }

        /// <summary>
        /// Mutation process
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        private List<Chromosome> Mutation(List<Chromosome> population)
        {
            List<Chromosome> newPopulation = new List<Chromosome>();

            foreach (Chromosome c in population)
            {
                Chromosome chr = new Chromosome(ChromosomeLength); // TODO try to remove

                for (int i = 0; i < ChromosomeLength; i++)
                {
                    double randomNumber = random.NextDouble();

                    if (randomNumber < MutationParamter)
                    {
                        c.Genes[i].ToggleValue();
                    }

                    chr.Genes[i].Value = c.Genes[i].Value;
                }

                newPopulation.Add(chr);
            }

            return newPopulation; // TODO return population
        }

        public double GetMeanValue(List<Chromosome> population)
        {
            double sum = 0.0;

            foreach (Chromosome c in CurrentPopulation)
            {
                sum = sum + c.RealFunctionValue;
            }

            return sum / CurrentPopulation.Count;
        }

        private void EvaluatePopulation(List<Chromosome> population)
        {
            double sum = 0.0;

            foreach (Chromosome c in population)
            {
                Tuple<double, double, double> coefficients = c.GetCoefficients();

                double realFunctionValue = 0.0;
                c.FunctionValue = ObjectiveFunction(coefficients, out realFunctionValue);
                c.RealFunctionValue = realFunctionValue;

                sum = sum + c.FunctionValue;
            }

            foreach (Chromosome c in population)
            {
                c.Fitness = c.FunctionValue / sum;
            }

            List<double> cumulatives = new List<double>
            {
                population[0].Fitness
            };

            for (int i = 1; i < PopulationCount; i++)
            {
                cumulatives.Add(cumulatives[i - 1] + population[i].Fitness);
            }

            for (int i = 0; i < PopulationCount; i++)
            {
                population[i].CumulativeFitness = cumulatives[i];
            }
        }

        private List<double> GenerateRandomSelectionNumbers(int populationNumber)
        {
            List<double> randomNr = new List<double>(populationNumber);

            for (int i = 0; i < populationNumber; i++)
            {
                double randomNumber = 1 - random.NextDouble();
                randomNr.Add(randomNumber);
            }

            return randomNr;
        }

        private double GetAssociatedValue(double a, double b, double chromosomeBase10, int length)
        {
            double associatedValue = 0.0;

            associatedValue = a + chromosomeBase10 * ((b - a) / (Math.Pow(2, length) - 1));

            return associatedValue;
        }

        private double ObjectiveFunction(Tuple<double, double, double> coefficients, out double realFunctionValue)
        {
            // calculate the sum
            double sum = 0.0;
            double bigValue = 10000.0;

            //List<double> hvalues = new List<double>();

            foreach (LearningData item in LearningData)
            {
                double h = coefficients.Item1 * 1 + coefficients.Item2 * item.BallDirection + coefficients.Item3 * item.BallYPosition;
                double y = item.PaddleYPosition;

                //hvalues.Add(h);

                sum += Math.Pow((h - y), 2);
            }

            sum /= (2 * LearningData.Count);
            realFunctionValue = sum;

            Debug.Assert((bigValue - sum) > 0);

            return (bigValue - sum);
        }

        #endregion

        #region events

        public delegate void GeneratedNewPopulationEventHandler(object sender, EventArgs e);

        public event GeneratedNewPopulationEventHandler GeneratedNewPopulation = delegate { };

        public delegate void ResetPopulationEventHandler(object sender, EventArgs e);

        public event ResetPopulationEventHandler ResetPopulation = delegate { };

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
