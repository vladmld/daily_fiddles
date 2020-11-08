using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace LearningDataProcessor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        private PointCollection meanValues;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
            this.viewModel.ResetPopulation += ResetMeanGraph;
            this.viewModel.GeneratedNewPopulation += DrawGraph;
            this.viewModel.GeneratedNewPopulation += RefreshUI;

            meanValues = new PointCollection();
            meanValues.Add(new Point { X = 0, Y = 0 });
            MeanValue.ItemsSource = meanValues;
        }

        private void RefreshUI(object sender, EventArgs e)
        {
            BtnNextGeneration.IsEnabled = true;
        }

        private void ResetMeanGraph(object sender, EventArgs e)
        {
            if (meanValues.Count > 1)
            {
                meanValues.Clear();
                meanValues.Add(new Point { X = 0, Y = 0 });
            }
        }

        private void DrawGraph(object sender, EventArgs e)
        {
            if (sender is List<Chromosome> population)
            {
                List<KeyValuePair<int, double>> valueList = new List<KeyValuePair<int, double>>();
                int step = 0;
                Chromosome bestChromosome = population.First();

                foreach (Chromosome c in population)
                {
                    if (c.Fitness > bestChromosome.Fitness)
                    {
                        bestChromosome = c;
                    }

                    valueList.Add(new KeyValuePair<int, double>(step, c.RealFunctionValue));
                    step += 10;
                }

                currentGenerationValues.DataContext = valueList;
                meanValues.Add(new Point { X = this.viewModel.GenerationNumber * 10, Y = this.viewModel.MeanValue });

                PointCollection pc = new PointCollection();
                pc.Add(new Point { X = 0, Y = this.viewModel.MeanValue });
                pc.Add(new Point { X = step, Y = this.viewModel.MeanValue });

                MeanValueLine.ItemsSource = pc;

                TbFittestChromosome.Text = String.Format("The best chromosome in this generation is {0}, with fitness value {1:0.000} and error function value {2:0.000}", bestChromosome.ToString(), bestChromosome.Fitness, bestChromosome.RealFunctionValue);
            }
        }
    }
}
