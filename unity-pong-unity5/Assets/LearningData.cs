using System;

namespace Assets
{
    [Serializable]
    public class LearningData
    {
        public double BallYPosition { get; set; }
        public double BallDirection { get; set; }
        public double PaddleYPosition { get; set; }
    }
}
