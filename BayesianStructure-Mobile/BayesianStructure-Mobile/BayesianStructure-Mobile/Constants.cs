using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BayesianStructure
{
    public static class Constants
    {
        public static bool SHOW_ERROR_MESSAGES = true;

        #region Constraints
        public const float GRAVITATIONAL_PULL = 9.80f; //9.8 m/s^2 
        public const float ACCELERATION_MAX = 1f; //1g
        public const float ACCELERATION_MIN = -1f; //-1g
 
        public const float JITTER_MAX = 0.1f; // 0.05g
        public const float JITTER_MIN = -0.1f; //-0.05g

        public const float POSITION_START = 1.5f; //1.5 m
        public static float POSITION_MAX = 3f; // 3 m
        public const float POSITION_MIN = 0f; //0 m

        public const float VELOCITY_MAX = 2f; // 3 m/s
        public const float VELOCITY_MIN = -2f; //-3m/s
        #endregion

        #region Velocity and position constants
        private const float _ballAffection = 0.0f;
        private const float _previousPositionAffection = 1.0f - _ballAffection;
        private const float _previousVelocityAffection = 1.0f;
        public const float DELTA_TIME = 0.02f;
        public static float[,] F_VELOCITY = new float[1, 2] { { DELTA_TIME, _previousVelocityAffection } };
        public static float[,] F_POSITION = new float[1, 3] { { DELTA_TIME, _previousPositionAffection, _ballAffection } };
        #endregion

        #region Moving average constants
        private const float _ALPHA = 0.095f;
        public static float[,] F_MOVING_AVERAGE = new float[1, 2] { { _ALPHA, (1f - _ALPHA) } };
        #endregion

        #region Variance gains
        public const float MOVING_AVERAGE_VARIANCE_GAIN = 0.1f; //0f;
        public const float VELOCITY_VARIANCE_GAIN = 0.1f;//0f;
        public const float POSITION_VARIANCE_GAIN = 0.1f;//0.000001f;
        public const float BALL_VARIANCE_GAIN = 0f;
        #endregion

        #region Acceleration variance
        public const float ACCELERATION_VARIANCE = 0.005523301f;
        #endregion

        #region Mean Gains
        public const float ACCELERATION_MEAN_GAIN = 0f;
        public const float VELOCITY_MEAN_GAIN = 0f;
        public const float POSITION_MEAN_GAIN = 0f;
	    #endregion

        #region Velocity Drift
        public const int CONSTANT_VELOCITY_LIMIT = 15;
        public const float CONSTANT_VELOCITY_JITTER_LIMIT = 0.003f;
        #endregion

        #region Base Direction
        public static float BASE_DIRECTION = 0f;
        #endregion
    }
}
