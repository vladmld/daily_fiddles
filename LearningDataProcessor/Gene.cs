namespace LearningDataProcessor
{
    public class Gene
    {
        public int Value { get; set; }

        public Gene()
        {
            Value = 0;
        }

        public Gene(int value)
        {
            if (value > 0)
            {
                Value = 1;
            }
            else
            {
                Value = 0;
            }
        }

        public void ToggleValue()
        {
            if (this.Value.Equals(0))
            {
                this.Value = 1;
            }
            else
            {
                this.Value = 0;
            }
        }
    }
}
