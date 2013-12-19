using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BayesianStructure
{
    public class Network
    {
        #region Properties
        public Variable Acceleration {get; private set; }
        public Variable Gyroscope { get; private set; }
        public Variable ATan2 { get; private set; }
        public Variable ComplementaryFilter { get; private set; }
        public Variable SensorFusion { get; private set; }
        public Variable MovingAverage {get; private set; }
        public Variable CorrectedMovingAverage {get; private set; }
        public Variable Velocity {get; private set; }
        public Variable Position {get; private set; }
        public Variable Ball {get; private set; }
        #endregion
     
        #region Constructors
        public Network()
        {
            Acceleration = new Acceleration(Vector3.Zero);
            Gyroscope = new Gyroscope(Vector3.Zero);
            ATan2 = new ATan2Angle(Acceleration);
            ComplementaryFilter = new ComplementaryFilter();
            SensorFusion = new SensorFusion(ComplementaryFilter, Acceleration);
            MovingAverage = new MovingAverage(0.0f, 0.0f);
            CorrectedMovingAverage = new CorrectedMovingAverage(this);
            Velocity = new Velocity(0.0f, 0.0f);
            Ball = new BallPositionDistribution(1.5f);
            Position = new Position(0.0f, Constants.POSITION_START);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Update network according to the newAcceleration
        /// </summary>
        /// <param name="newAcceleration">Vector3 Accelerometer reading here</param>
        /// <param name="newBallPositionX">The ball's position in meters here(X Axis)</param>
        /// <param name="newGyroscope">Vector3 Gyroscope reading here</param>
        public void UpdateNetwork(Vector3 newAcceleration, float newBallPositionX, Vector3 newGyroscope)
        {
            Acceleration = new Acceleration(newAcceleration);
            Gyroscope = new Gyroscope(newGyroscope);
            ATan2 = new ATan2Angle(Acceleration);
            ComplementaryFilter = new ComplementaryFilter(ATan2, Gyroscope, ComplementaryFilter);
            SensorFusion = new SensorFusion(ComplementaryFilter, Acceleration);
            MovingAverage = new MovingAverage(MovingAverage, SensorFusion);
            CorrectedMovingAverage = new CorrectedMovingAverage(MovingAverage);
            Velocity = new Velocity(Velocity, CorrectedMovingAverage);
            Ball = new BallPositionDistribution(newBallPositionX);
            Position = new Position(Position, Velocity, Ball);
        }
        #endregion
    }
}

