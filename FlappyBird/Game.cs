using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace FlappyBird
{

  public partial class Game : Form
  {
	private const int PIPE_COIN_GROUND_SPEED = 7;
	private const string FONT_FAMILY = "Microsoft Sans Serif";
	const float GAME_OVER_SCREEN_HEIGHT_PERCENTAGE_FOR_POSITION_Y = 0.1f;

	private bool _keyDown = false;
	private bool _jumped = false;
	private bool _gameStarted = false;

	private const int FLAP_INTERVAL = 3;
	private int _flapIntervalCounter = 0;
	private int _birdFlapImageCounter = 0;

	private const int JUMP_INTERVAL = 9;
	private int _jumpIntervalCounter = 0;

	private Bird _bird;
	private Background _background;
	private Ground _ground;

	private List<Pipes> _pipes;
	private List<Coin> _coins;

	private const int PIPE_INTERVAL = 35;
	private int _pipeIntervalCounter = 0;
	private int _playableBackgroundHeight;

	private bool _gameOver = false;
	private const int DEAD_BIRD_POSITION_X_OFFSET = 25;
	private bool _isBirdRotated = false;
	private int _score;
	private int _highScore;

	// To play multiple sound at the same time
	private System.Windows.Media.MediaPlayer _mediaPlayer;
	private System.Windows.Media.MediaPlayer _scoreSound;

	public Game()
	{
	  InitializeComponent();
	  _background = new Background(_pictureBox.Width, _pictureBox.Height);
	  _ground = new Ground(_pictureBox.Width, _pictureBox.Height, PIPE_COIN_GROUND_SPEED);
	  _playableBackgroundHeight = _pictureBox.Height - _ground.Height;
	  // The following code was copied from https://stackoverflow.com/questions/6240002/play-two-sounds-simultaneusly
	  _mediaPlayer = new System.Windows.Media.MediaPlayer();
	  _scoreSound = new System.Windows.Media.MediaPlayer();
	  // End of copied code
	  _highScore = 0;

	  CreateRestartButton();
	  Init();
	}

	private void Init()
	{
	  _pipes = new List<Pipes>();
	  _coins = new List<Coin>();
	  _bird = new Bird(_pictureBox.Width, _pictureBox.Height);
	  CreateAndAddPipeAndCoinToList();
	  _gameOver = false;
	  _isBirdRotated = false;
	  _pipeIntervalCounter = 0;
	  _jumped = false;
	  _jumpIntervalCounter = 0;
	  _score = 0;
	  HideRestartButton();
	  _flappyBirdTimer.Start();
	  _pictureBox.Refresh();
	}

	private void PictureBoxPaint(object sender, PaintEventArgs e)
	{
	  Graphics graphics = e.Graphics;

	  _background.Display(graphics);
	  _ground.Display(graphics);
	  foreach (Pipes pipe in _pipes)
	  {
		pipe.Display(graphics);
	  }
	  foreach (Coin coin in _coins)
	  {
		coin.Display(graphics);
	  }

	  if (!_gameStarted)
	  {
		const float LOGO_SCREEN_HEIGHT_PERCENTAGE_FOR_POSITION_Y = 0.03f;
		int logoPositionY = (int)(_pictureBox.Height * LOGO_SCREEN_HEIGHT_PERCENTAGE_FOR_POSITION_Y);
		DisplayImage(graphics, Properties.Resources.logo, logoPositionY);
		const int GET_READY_POSITION_Y_OFFSET = 30;
		DisplayImage(graphics, Properties.Resources.get_ready, 
		  logoPositionY + Properties.Resources.logo.Height + GET_READY_POSITION_Y_OFFSET);

		DisplayInstructions(graphics);
	  }
	  else
	  {
		if (!_gameOver)
		{
		  const float SCREEN_TOP_SCORE_SCREEN_WIDTH_PERCENTAGE_FOR_POSITION_X = 0.04f;
		  const int GAME_OVER_FONT_SIZE = 20;
		  DrawString(graphics, _score.ToString(), GAME_OVER_FONT_SIZE, Color.White, 
			(int)(_pictureBox.Width * SCREEN_TOP_SCORE_SCREEN_WIDTH_PERCENTAGE_FOR_POSITION_X));
		}
	  }

	  const int BIRD_ROTATE_ANGLE = 45;
	  RotateBirdAtAngle(graphics, BIRD_ROTATE_ANGLE);
	  _bird.Display(graphics);
	  graphics.ResetTransform();

	  if (_gameOver)
	  {
		int gameOverImagePosiyionY = (int)(_pictureBox.Height * GAME_OVER_SCREEN_HEIGHT_PERCENTAGE_FOR_POSITION_Y);
		DisplayImage(graphics, Properties.Resources.game_over, gameOverImagePosiyionY);
		DisplayGameOverScore(graphics, gameOverImagePosiyionY);
	  }
	}

	private void RotateBirdAtAngle(Graphics graphics, int angle)
	{
	  if (_gameStarted)
	  {
		PointF centre = new PointF((float)(_bird.X + (_bird.Width / 2)), (float)(_bird.Y + (_bird.Height / 2)));
		Matrix matrix = new Matrix();
		if (_jumped)
		{
		  angle = -angle;
		}
		
		matrix.RotateAt(angle, centre);
		graphics.Transform = matrix;
	  }
	}

	private void DisplayImage(Graphics graphics, Bitmap image, int y)
	{
	  int width = image.Width;
	  int height = image.Height;
	  int x = (_pictureBox.Width - width) / 2;
	  graphics.DrawImage(image, x, y, width, height);
	}

	private void CreateRestartButton()
	{
	  int gameOverheight = Properties.Resources.game_over.Height;

	  Button restartButton = new Button();
	  restartButton.Height = 40;
	  restartButton.Width = 120;
	  restartButton.BackColor = Color.OrangeRed;
	  restartButton.ForeColor = Color.White;
	  restartButton.FlatAppearance.BorderColor = Color.Brown;
	  restartButton.FlatAppearance.BorderSize = 3;
	  restartButton.FlatStyle = FlatStyle.Flat;
	  restartButton.Font = new Font(FONT_FAMILY, 14, FontStyle.Bold);
	  restartButton.Text = "Restart";
	  restartButton.Name = "_restartButton";
	  restartButton.Cursor = Cursors.Hand;

	  int x = (_pictureBox.Width - restartButton.Width) / 2;
	  const int POSITION_Y_OFFSET = 10;
	  int y = (int)(_pictureBox.Height * GAME_OVER_SCREEN_HEIGHT_PERCENTAGE_FOR_POSITION_Y) + 
		gameOverheight + POSITION_Y_OFFSET;
	  restartButton.Location = new Point(x, y);
	  restartButton.Click += new EventHandler(RestartButtonClick);
	  _pictureBox.Controls.Add(restartButton);
	}

	private void RestartButtonClick(object sender, EventArgs e)
	{
	  Init();
	}

	private void DisplayGameOverScore(Graphics graphics, int gameOverImagePositionY)
	{
	  const int FONT_SIZE = 12;
	  Color fontColor = Color.White;
	  const int GAMEOVER_SCORE_POSITION_X_OFFSET = 20;
	  const int GAMEOVER_SCORE_POSITION_Y_OFFSET = 75;
	  int x = ((_pictureBox.Width - Properties.Resources.game_over.Width) / 2) + GAMEOVER_SCORE_POSITION_X_OFFSET;
	  int y = (int)(gameOverImagePositionY + GAMEOVER_SCORE_POSITION_Y_OFFSET);
	  DrawString(graphics, _score.ToString(), FONT_SIZE, fontColor, y, x);
	  const int GAMEOVER_HI_SCORE_POSITION_Y_OFFSET = 118;
	  y = (int)(gameOverImagePositionY + GAMEOVER_HI_SCORE_POSITION_Y_OFFSET);
	  DrawString(graphics, _highScore.ToString(), FONT_SIZE, fontColor, y, x);
	}

	private void DisplayInstructions(Graphics graphics)
	{
	  string text = "Press SPACE BAR to jump";
	  Color fontColor = Color.OrangeRed;
	  const int FONT_SIZE = 15;
	  const int POSITION_Y_OFFSET = 60;
	  Font font = new Font(FONT_FAMILY, FONT_SIZE, FontStyle.Bold);
	  Size textSize = TextRenderer.MeasureText(text, font);
	  int y = (int)(_bird.Y + _bird.Height + POSITION_Y_OFFSET);
	  DrawString(graphics, text, FONT_SIZE, fontColor, y);

	  fontColor = Color.White;
	  text = "1 point for crossing the pipes";
	  y = y + textSize.Height;
	  int x = (_pictureBox.Width - textSize.Width) / 2;
	  DrawString(graphics, text, FONT_SIZE, fontColor, y, x);

	  const int POSITION_X_Y_OFFSET = 5;
	  int positionYOffset = textSize.Height + POSITION_X_Y_OFFSET;
	  int textPositionX = x + textSize.Height + POSITION_X_Y_OFFSET;
	  Bitmap image = Properties.Resources.bronze_coin;
	  text = "2 points";

	  y = CreateCoinInstruction(graphics, x, y + positionYOffset, 
		textSize.Height, text, image);
	  DrawString(graphics, text, FONT_SIZE, fontColor, y, textPositionX);

	  image = Properties.Resources.silver_coin;
	  text = "5 points";
	  y = CreateCoinInstruction(graphics, x, y + positionYOffset,
		textSize.Height, text, image);
	  DrawString(graphics, text, FONT_SIZE, fontColor, y, textPositionX);

	  image = Properties.Resources.gold_coin;
	  text = "10 points";
	  y = CreateCoinInstruction(graphics, x, y + positionYOffset,
		textSize.Height, text, image);
	  DrawString(graphics, text, FONT_SIZE, fontColor, y, textPositionX);
	}

	private int CreateCoinInstruction(Graphics graphics, int x, int y, int textHeight, string text, Bitmap image)
	{
	  // The following code was copied from https://docs.microsoft.com/en-us/dotnet/api/system.drawing.image.fromfile?view=netframework-4.8
	  Rectangle rectangle = new Rectangle(x, y, textHeight, textHeight);
	  graphics.DrawImage(image, rectangle);
	  // End of copied code
	  return rectangle.Y;
	}

	private void DrawString(Graphics graphics, string text, int fontSize, Color fontColor, int y, int x = 0)
	{
	  Brush brush = new SolidBrush(fontColor);
	  Font font = new Font(FONT_FAMILY, fontSize, FontStyle.Bold);
	  Size textSize = TextRenderer.MeasureText(text, font);
	  if (x == 0)
	  {
		x = (_pictureBox.Width - textSize.Width) / 2;
	  }
	  graphics.DrawString(text, font, brush, x, y);
	}

	private void FlappyBirdTimerTick(object sender, EventArgs e)
	{
	  if (!_gameOver)
	  {
		Flap();
		_background.Move();
		_ground.Move();
	  }

	  if (_gameStarted)
	  {
		if (!_gameOver)
		{
		  Jump();
		  MovePipes();
		  MoveCoins();
		  CreatePipesAndCoins();
		}
		else
		{
		  ShowRestartButton();
		  GameOver();
		}
	  }

	  _pictureBox.Refresh();
	}

	private void Flap()
	{
	  _flapIntervalCounter++;
	  if (_flapIntervalCounter == FLAP_INTERVAL)
	  {
		_bird.SetBirdImage(_birdFlapImageCounter);
		if (_birdFlapImageCounter == 3)
		{
		  _birdFlapImageCounter = 0;
		}
		_flapIntervalCounter = 0;
		_birdFlapImageCounter++;
	  }
	}

	private void Jump()
	{
	  if (_jumped)
	  {
		if (_jumpIntervalCounter <= JUMP_INTERVAL)
		{
		  _bird.Move();
		}
		else
		{
		  _jumped = false;
		  _jumpIntervalCounter = 0;
		}
		_jumpIntervalCounter++;
	  }
	  else
	  {
		Fall();
	  }
	}

	private void Fall()
	{
	  _bird.Fall();
	  if (_ground.Collided(_bird))
	  {
		_gameOver = true;
		PlaySound("hit");
		UpdateHighScore();
	  }
	}

	private void MovePipes()
	{
	  List<Pipes> keptPipes = new List<Pipes>();
	  foreach (Pipes pipe in _pipes)
	  {
		pipe.Move();
		if (pipe.Collided(_bird) || pipe.OutOfScreenBirdCrossedPipe(_bird))
		{
		  _gameOver = true;
		  PlaySound("hit");
		  UpdateHighScore();
		}
		pipe.CheckPipeCrossed(_bird);
		UpdateScore(pipe);
		if (!pipe.OutOfScreen())
		{
		  keptPipes.Add(pipe);
		}
	  }
	  _pipes = keptPipes;
	}

	private void MoveCoins()
	{
	  List<Coin> keptCoins = new List<Coin>();
	  foreach (Coin coin in _coins)
	  {
		coin.Move();
		if (coin.Collided(_bird))
		{
		  _score += coin.Score;
		  PlayScoreSound("point");
		}
		else
		{
		  keptCoins.Add(coin);
		}
	  }
	  _coins = keptCoins;
	}

	private void CreatePipesAndCoins()
	{
	  if (_pipeIntervalCounter > PIPE_INTERVAL)
	  {
		CreateAndAddPipeAndCoinToList();
		_pipeIntervalCounter = 0;
	  }
	  _pipeIntervalCounter++;
	}

	private void CreateAndAddPipeAndCoinToList()
	{
	  _pipes.Add(new Pipes(_pictureBox.Width, _playableBackgroundHeight, PIPE_COIN_GROUND_SPEED));
	  _coins.Add(new Coin(_pictureBox.Width, _playableBackgroundHeight, PIPE_COIN_GROUND_SPEED));
	}

	private void GameOver()
	{
	  if (!_ground.Collided(_bird))
	  {
		if (!_isBirdRotated)
		{
		  _bird.X += DEAD_BIRD_POSITION_X_OFFSET;
		  _bird.GetBirdImage().RotateFlip(RotateFlipType.Rotate90FlipNone);
		}
		_isBirdRotated = true;
		_bird.Fall();
	  }
	  else
	  {
		_flappyBirdTimer.Stop();
		_bird.GetBirdImage().RotateFlip(RotateFlipType.Rotate90FlipNone);
	  }
	}

	private void GameKeyDown(object sender, KeyEventArgs e)
	{
	  if (e.Modifiers == Keys.None && e.KeyCode == Keys.Space && !_keyDown && !_gameOver && !_jumped)
	  {
		PlaySound("wing");
		_keyDown = true;
		_jumped = true;
		if (!_gameStarted)
		{
		  _gameStarted = true;
		}
	  }
	  e.SuppressKeyPress = true;
	}

	private void GameKeyUp(object sender, KeyEventArgs e)
	{
	  if (e.Modifiers == Keys.None && _keyDown)
	  {
		_keyDown = false;
	  }
	}

	private void PlaySound(string fileName)
	{
	  string filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../Resources/" + fileName + ".wav");
	  _mediaPlayer.Open(new Uri(filePath));
	  _mediaPlayer.Play();
	}

	private void UpdateScore(Pipes pipe)
	{
	  if (pipe.PipeCrossed && !pipe.PipeScored)
	  {
		_score++;
		// PlayScoreSound("point");
	  }
	}

	private void PlayScoreSound(string fileName)
	{
	  string filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../Resources/" + fileName + ".wav");
	  _scoreSound.Open(new Uri(filePath));
	  _scoreSound.Play();
	}

	private void UpdateHighScore()
	{
	  if (_score > _highScore)
	  {
		_highScore = _score;
		PlayScoreSound("high-score");
	  }
	}

	private void ShowRestartButton()
	{
	  _pictureBox.Controls["_restartButton"].Enabled = true;
	  _pictureBox.Controls["_restartButton"].Visible = true;
	}

	private void HideRestartButton()
	{
	  _pictureBox.Controls["_restartButton"].Enabled = false;
	  _pictureBox.Controls["_restartButton"].Visible = false;
	}

  }
}
