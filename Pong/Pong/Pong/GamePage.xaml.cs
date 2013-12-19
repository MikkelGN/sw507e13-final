using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Devices.Sensors;
using Pong.PowerUps;
using BayesianStructure;

namespace Pong
{
    public partial class GamePage : PhoneApplicationPage
    {
        #region Properties
        //FPS - First Person Shooter
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        float _fps = 0;
        string fedt = "";

        //Services
        private ContentManager _contentManager;
        private GameTimer _timer;
        private SpriteBatch _spriteBatch;
        private Accelerometer _accelerometer = new Accelerometer();
        private Compass _compass = new Compass();
        private Gyroscope _gyroscope = new Gyroscope();
        private Viewport _viewPort;

        // Movement vars
        private const int INTERVAL_BETWEEN_STEPS = 10;
        private const float PADDLE_MOVEMENT_MULTIPLIER = 4000f;
        private Vector2 _paddleVelocity = Vector2.Zero;
        private Vector2 _paddleAcceleration = Vector2.Zero;
        private Vector2 _paddlePosition = Vector2.Zero;
        private float _maxStepsRight;
        private float _maxStepsLeft;
        private int _countZeroAcceleration = 0;
        private const int COUNT_ZERO_ACCELERATION_LIMIT = 10;
        private float _countLeftAcceleration = 0f;
        private float _countRightAcceleration = 0f;

        // Ball vars
        private bool DisableBall = false;
        private List<Ball> _balls = new List<Ball>();
        private const int BASE_BALL_SPEED = 100;
        private float _ballSpeedMultiplier = 1f;
        private const float BALL_SPEED_MULTIPLIER_FACTOR = 1.2f;

        // Paddle vars
        private Texture2D _paddle;
        private const int BASE_PADDLE_WIDTH = 80;
        private const int BASE_PADDLE_HEIGHT = 20;
        private bool _boundingBoxActive = true;
        private const int MAX_PADDLE_WIDTH = 240;
        private const int MIN_PADDLE_WIDTH = 40;
        private int _currentPaddleWidth;
        private int _paddleYPosition;
        private int PosMax;

        // Level vars
        private const double TIME_BEFORE_LEVELUP = 10f;
        private double _timeLevel = 0f;

        //levelup vars
        private bool _showLevelUpText = false;
        private const double TIME_TO_SHOW_LEVELUP_TEXT = 2;
        private double _timeShownLevelUpText = 0;
        private const string LEVELUP_TEXT = "Level Up!";
        private Vector2 _levelUpTextPosition;

        //fonts
        private SpriteFont _scoreFont;
        private Vector2 _scorePosition;
       
        //powerups
        private List<InstantiatedPowerUp> _instantiatedPowerUps = new List<InstantiatedPowerUp>();
        private Random _randomizer = new Random();
        private const int POWERUP_WIDTH = 24;
        private const int POWERUP_HEIGHT = 24;
        private double _lastSpawnedPowerUpTime = 0;
        private double _frequencySpawningPowerupTime = new Random().Next(4,13);
        private float _powerUpFalldownSpeed = 5f;

        private Network _theNetwork;
        #endregion
        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            _contentManager = (Application.Current as App).Content;

            // Create a timer for this page
            _timer = new GameTimer();
            _timer.UpdateInterval = TimeSpan.FromTicks(333333);
            _timer.Update += OnUpdate;
            _timer.Draw += OnDraw;
           
            _gyroscope.TimeBetweenUpdates = TimeSpan.FromMilliseconds((int)(Constants.DELTA_TIME * 1000));
            _compass.TimeBetweenUpdates = TimeSpan.FromMilliseconds((int)(Constants.DELTA_TIME * 1000));
            _accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds((int)(Constants.DELTA_TIME * 1000));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);
            _viewPort = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport;
            _spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);
            
            _scoreFont = _contentManager.Load<SpriteFont>("scoreFont");
            _scorePosition = new Vector2(20, 10);
            _levelUpTextPosition = new Vector2(_viewPort.Width / 2 - 150, _viewPort.Height / 2 - 60);

            _currentPaddleWidth = BASE_PADDLE_WIDTH;
            _paddleYPosition = _viewPort.Height - BASE_PADDLE_HEIGHT * 2;
            _paddlePosition.X = _viewPort.Width / 2 - BASE_PADDLE_WIDTH / 2;

            _paddle = _contentManager.Load<Texture2D>("paddle");

            _balls.Add(new Ball(_contentManager));
            _balls[0].Position = new Vector2(_randomizer.Next(0, _viewPort.Width - _balls[0].BallTexture.Width), _paddleYPosition - 30);
            _balls[0].Speed = new Vector2(-BASE_BALL_SPEED, _randomizer.Next(0, 1) == 0 ? -BASE_BALL_SPEED : BASE_BALL_SPEED);

            GlobalVariables.BallWidth = _balls[0].BallTexture.Width;

            _theNetwork = new Network();



            #region PowerUps Initialization
            if (!PowerUpHelper.Initialized)
            {
                // increase paddle size
                PowerUp increasePaddleSize = new IncreasePaddleSize(2f);
                increasePaddleSize.Texture = _contentManager.Load<Texture2D>("IncreasePaddleSize");
                PowerUpHelper.PowerUps.Add(increasePaddleSize);

                // decrease paddle size
                PowerUp decreasePaddleSize = new IncreasePaddleSize(0.5f);
                decreasePaddleSize.Texture = _contentManager.Load<Texture2D>("DecreasePaddleSize");
                PowerUpHelper.PowerUps.Add(decreasePaddleSize);
                
                //// split ball to Two
                //PowerUp splitBallTwo = new SplitBall(1);
                //splitBallTwo.Texture = _contentManager.Load<Texture2D>("SplitToTwo");
                //PowerUpHelper.PowerUps.Add(splitBallTwo);

                //// split ball to Three
                //PowerUp splitBallThree = new SplitBall(2);
                //splitBallThree.Texture = _contentManager.Load<Texture2D>("SplitToTwo");
                //PowerUpHelper.PowerUps.Add(splitBallThree);
                
                //// split ball to Four
                //PowerUp splitBallFour = new SplitBall(3);
                //splitBallFour.Texture = _contentManager.Load<Texture2D>("SplitToTwo");
                //PowerUpHelper.PowerUps.Add(splitBallFour);
                
                //Increase Ball Speed
                PowerUp increaseBallSpeed = new ChangeBallSpeed(1.5f);
                increaseBallSpeed.Texture = _contentManager.Load<Texture2D>("SpeedUp");
                PowerUpHelper.PowerUps.Add(increaseBallSpeed);

                //decrease Ball Speed
                PowerUp decreaseBallSpeed = new ChangeBallSpeed(0.5f);
                decreaseBallSpeed.Texture = _contentManager.Load<Texture2D>("SlowDown");
                PowerUpHelper.PowerUps.Add(decreaseBallSpeed);

                //Add points
                PowerUp addPoints = new ChangePoints(2f);
                addPoints.Texture = _contentManager.Load<Texture2D>("AddPoints");
                PowerUpHelper.PowerUps.Add(addPoints);

                //Remove points
                PowerUp removePoints = new ChangePoints(0.5f);
                removePoints.Texture = _contentManager.Load<Texture2D>("RemovePoints");
                PowerUpHelper.PowerUps.Add(removePoints);
                
                PowerUpHelper.Initialized = true;
            }
            #endregion

            GlobalVariables.Reset();

            _accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);
            _compass.Start();
            _gyroscope.Start();
            _accelerometer.Start();
            

            // Start the timer
            _timer.Start();

            base.OnNavigatedTo(e);
            
        }
   
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            _timer.Stop();
            _accelerometer.Stop();
            _compass.Stop();
            _gyroscope.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);
            //accelerometer.Stop();
            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            
            BayesianStructure.Constants.POSITION_MAX = 3f - (3f * (float)_currentPaddleWidth / (float)_viewPort.Width);
            // TODO: Add your update logic here
            UpdatePaddlePosition(e.ElapsedTime);
            //RunTestData(e.TotalTime);
            
            UpdateBallPosition(e.ElapsedTime);
            CheckForBallPaddleCollision();
            CheckForPaddlePowerUpCollision();
            IncreaseScore(e.ElapsedTime);

            CheckForNextLevel(e.ElapsedTime);
            CheckForLevelUpTextVisibility(e.ElapsedTime);
            CheckForPowerUpTimer(e.TotalTime);
            UpdatePowerUpPositions(e.ElapsedTime);
            DeletePowerUps();


             // Update

            _fps = 1f / (float)e.ElapsedTime.TotalSeconds;


        }

        private void DeletePowerUps()
        {
            foreach (var powerup in _instantiatedPowerUps.Where(x => x.Delete).ToList())
            {
                _instantiatedPowerUps.Remove(powerup);
            }
        }

        private void UpdatePowerUpPositions(TimeSpan gameTime)
        {
            for(int i = 0; i < _instantiatedPowerUps.Count; i++)
            {
                _instantiatedPowerUps[i].Position = _instantiatedPowerUps[i].Position + new Vector2(0f, _powerUpFalldownSpeed);
                if (_instantiatedPowerUps[i].Position.Y > _viewPort.Height)
                {
                    _instantiatedPowerUps[i].Delete = true;
                }
            }
        }

        private void CheckForPowerUpTimer(TimeSpan gameTime)
        {
            if (gameTime.TotalSeconds > _lastSpawnedPowerUpTime + _frequencySpawningPowerupTime)
            {
                _lastSpawnedPowerUpTime = gameTime.TotalSeconds;
                InstantiatePowerUp();
                _frequencySpawningPowerupTime = new Random().Next(4, 13);
            }
            
        }

        private void CheckForLevelUpTextVisibility(TimeSpan gameTime)
        {
            if (_showLevelUpText)
            {
                if (_timeShownLevelUpText > TIME_TO_SHOW_LEVELUP_TEXT)
                {
                    _showLevelUpText = false;
                    _timeShownLevelUpText = 0;
                }
                else
                {
                    _timeShownLevelUpText += gameTime.TotalSeconds;
                }
            }
        }

        private void CheckForNextLevel(TimeSpan gameTime)
        {
            if (_timeLevel > TIME_BEFORE_LEVELUP)
            {
                _timeLevel = 0;
                LevelUp();
            }
            else
            {
                _timeLevel += gameTime.TotalSeconds;
            }
        }

        private void LevelUp()
        {
            _ballSpeedMultiplier *= BALL_SPEED_MULTIPLIER_FACTOR;
            _showLevelUpText = true;
        }

        private void IncreaseScore(TimeSpan gameTime)
        {
            GlobalVariables.Score += gameTime.TotalSeconds * _ballSpeedMultiplier;
        }

        private void CheckForBallPaddleCollision()
        {
            foreach (Ball b in _balls)
            {
                BoundingSphere ballSphere = new BoundingSphere(
                    new Vector3(b.Position.X + (b.BallTexture.Width / 2), 
                                b.Position.Y + (b.BallTexture.Height / 2), 0), b.BallTexture.Width / 2
                    );

                BoundingBox paddleBox = new BoundingBox(
                    new Vector3(_paddlePosition.X, _paddleYPosition, 0),
                    new Vector3(_paddlePosition.X + _currentPaddleWidth, _paddleYPosition + 1.0f, 0));

                BoundingBox paddleBottomBox = new BoundingBox(
                    new Vector3(_paddlePosition.X, _paddleYPosition + 1.0f, 0),
                    new Vector3(_paddlePosition.X + _currentPaddleWidth, _paddleYPosition + BASE_PADDLE_HEIGHT - 1.0f, 0));

                if (ballSphere.Intersects(paddleBox) && _boundingBoxActive)
                {
                    b.Speed.Y *= -1.0f;
                    b.Position.Y = _paddleYPosition - b.BallTexture.Height;
                }
                else if (ballSphere.Intersects(paddleBottomBox))
                {
                    _boundingBoxActive = false;
                    b.Speed.X *= -1.0f;
                    b.Position.X = b.Position.X < _paddlePosition.X ? _paddlePosition.X - b.BallTexture.Width : _paddlePosition.X + _paddle.Width;
                }
            }
        }

        private void CheckForPaddlePowerUpCollision()
        {
            if (_instantiatedPowerUps.Count > 0)
            {
                BoundingBox paddleBox = new BoundingBox(
                    new Vector3(_paddlePosition.X, _paddleYPosition + 1.0f, 0),
                    new Vector3(_paddlePosition.X + _currentPaddleWidth, _paddleYPosition + BASE_PADDLE_HEIGHT, 0));


                foreach (var powerUp in _instantiatedPowerUps)
                {
                    BoundingSphere powerUpSphere = new BoundingSphere(
                    new Vector3(powerUp.Position.X + (POWERUP_WIDTH / 2), powerUp.Position.Y + (POWERUP_HEIGHT / 2), 0),
                    GlobalVariables.BallWidth / 2
                    );

                    if (powerUpSphere.Intersects(paddleBox))
                    {
                        powerUp.Delete = true;
                        ApplyPowerUpEffect(powerUp.PowerUp);
                    }
                }

            }
        }

        private void ApplyPowerUpEffect(PowerUp powerUp)
        {
            #region IncreasePaddleSize
            if (powerUp is IncreasePaddleSize){

                IncreasePaddleSize effect = (IncreasePaddleSize)powerUp;
                int newPaddleWidth = (int)(_currentPaddleWidth * effect.IncreaseFactor);

                if (newPaddleWidth > MAX_PADDLE_WIDTH)
                {
                    _currentPaddleWidth = MAX_PADDLE_WIDTH;
                    
                }
                else if (newPaddleWidth < MIN_PADDLE_WIDTH)
                {
                    _currentPaddleWidth = MIN_PADDLE_WIDTH;
                }
                else
                {
                    if (newPaddleWidth < _currentPaddleWidth)
                    {
                        _paddlePosition.X += (_currentPaddleWidth - newPaddleWidth) / 2;
                    }
                    else
                    {
                        _paddlePosition.X -= (newPaddleWidth - _currentPaddleWidth) / 2;
                    }
                    _currentPaddleWidth = newPaddleWidth;
                }
            }
            #endregion
            #region SplitBall
            else if (powerUp is SplitBall)
            {
                SplitBall effect = (SplitBall)powerUp;
                for (int i = 0; i <= effect._ballAmount; i++)
                {
                    int count = _balls.Count;
                    for (int j = 0; j < count; j++)
                    {
                        if (i == 1)
                        {
                            _balls.Add(new Ball(_balls[j].Position, new Vector2(_balls[j].Speed.X, -_balls[j].Speed.Y), _contentManager));
                        }
                        else if (i == 2)
                        {
                            _balls.Add(new Ball(_balls[j].Position, new Vector2(-_balls[j].Speed.X, _balls[j].Speed.Y), _contentManager));
                        }
                        else if (i == 3)
                        {
                            _balls.Add(new Ball(_balls[j].Position, new Vector2(-_balls[j].Speed.X, -_balls[j].Speed.Y), _contentManager));
                        }
                    }
                }
            }
            #endregion
            #region ChangeBallSpeed
            else if (powerUp is ChangeBallSpeed)
            {
                ChangeBallSpeed effect = (ChangeBallSpeed)powerUp;
                foreach (Ball b in _balls)
                {
                    if (b.Speed.Y > BASE_BALL_SPEED * effect.speedAmount)
                    {
                        b.Speed *= effect.speedAmount;
                    }
                }
            }
            #endregion
            #region ChangeScore
            else if(powerUp is ChangePoints)
            {
                ChangePoints effect = (ChangePoints)powerUp;
                GlobalVariables.Score *= effect.scoreMultiplier;
            }
            #endregion
        }

        private void UpdateBallPosition(TimeSpan gameTime)
        {
            List<Ball> removeableBalls = new List<Ball>();
            foreach (Ball b in _balls)
            {

                b.Position += b.Speed * _ballSpeedMultiplier * (float)gameTime.TotalSeconds;

                int maxX = _viewPort.Width - b.BallTexture.Width;
                int minX = 0;
                int maxY = _viewPort.Height - b.BallTexture.Height;
                int minY = 0;

                //Check for bounce
                if (b.Position.X > maxX)
                {
                    b.Speed.X *= -1f;
                    b.Position.X = maxX;
                }
                else if (b.Position.X < minX)
                {
                    b.Speed.X *= -1f;
                    b.Position.X = minX;
                }
                if (b.Position.Y > maxY)
                {
                    removeableBalls.Add(b);
                }
                else if (b.Position.Y < minY)
                {
                    b.Speed.Y *= -1f;
                    b.Position.Y = minY;
                }
                if(DisableBall)
                    b.Position.Y = 13;
            }

            foreach(Ball b in removeableBalls){
                _balls.Remove(b);
            }
            removeableBalls.Clear();

            if (_balls.Count == 0)
            {
                NavigationService.Navigate(new Uri("/GameOver.xaml", UriKind.Relative)); 
            }
            
        }

        private void UpdatePaddlePosition(TimeSpan period)
        {
            float metersConvertedToPixels = _viewPort.Width / 3f;
            _paddlePosition.X = _theNetwork.Position.Mean * metersConvertedToPixels;
        }

        private void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            if (_balls.Count > 0)
            {   
                _theNetwork.UpdateNetwork(e.SensorReading.Acceleration, _balls[0].Position.X * (3.0f / 800.0f), _gyroscope.CurrentValue.RotationRate);
            }
        }
    
        private void InstantiatePowerUp()
        {
            InstantiatedPowerUp instance = new InstantiatedPowerUp();
            instance.PowerUp = PowerUpHelper.GetRandomPowerUp();
            instance.Position = new Vector2(_randomizer.Next(0, _viewPort.Width - POWERUP_WIDTH), -POWERUP_HEIGHT);
            _instantiatedPowerUps.Add(instance);
        }
            
        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.Black);
     
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
     
            // Draw Paddle
            _spriteBatch.Draw(_paddle, new Microsoft.Xna.Framework.Rectangle((int)_paddlePosition.X, _paddleYPosition, _currentPaddleWidth, BASE_PADDLE_HEIGHT), Color.White);

            // Draw Balls
            foreach (Ball b in _balls)
            {
                _spriteBatch.Draw(b.BallTexture, b.Position, Color.White);
            }
            // Draw text
            DrawLevelUpText(_spriteBatch);
            _spriteBatch.DrawString(_scoreFont, "Score: " + GlobalVariables.Score.ToString("n0"), _scorePosition, Color.Yellow);
           
            // Draw PowerUps
            DrawPowerUps(_spriteBatch);
           
            // Draw elements in queue

            _spriteBatch.DrawString(_scoreFont, "Pos: " + _theNetwork.Position.Mean.ToString(), new Vector2(100, 100), Color.Red);
            //_spriteBatch.DrawString(_scoreFont,  "fedt: " + fedt.ToString(), new Vector2(100, 140), Color.Red);
            //spriteBatch.DrawString(scoreFont, "Avg: " + GlobalVariables.AverageOffsetY.ToString("n3"), new Vector2(100, 180), Color.Red);
            _spriteBatch.End();
        }

        private void DrawLevelUpText(SpriteBatch spriteBatch)
        {
            if (_showLevelUpText)
            {
                spriteBatch.DrawString(_scoreFont, LEVELUP_TEXT, _levelUpTextPosition, Color.YellowGreen);
            }
        }

        private void DrawPowerUps(SpriteBatch spriteBatch)
        {
            foreach (var powerup in _instantiatedPowerUps)
            {
                spriteBatch.Draw(powerup.PowerUp.Texture, powerup.Position, Color.White);
            }
        }

        private Vector2 GetRandomRectangle()
        {
            Random random = new Random();
            return new Vector2(random.Next(0, _viewPort.Width), 0);

        }
    }
}