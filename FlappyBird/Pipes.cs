using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
  class Pipes
  {
    private const int EXTRA_PIPE_WIDTH = 20;
    private static readonly int WIDTH = Properties.Resources.top_pipe.Width + EXTRA_PIPE_WIDTH;
    private const int TOP_PIPE_POSITION_Y = 0;
    private const int VERTICAL_GAP_BETWEEN_PIPES = 140;
    private const float PLAYABLE_BACKGROUND_HEIGHT_PERCENTAGE_FOR_MINIMUM_HEIGHT = 0.15f;
    private const float PLAYABLE_BACKGROUND_HEIGHT_PERCENTAGE_FOR_MAXIMUM_HEIGHT = 0.85f;

    private int _speed;
    private Pipe _topPipe;
    private Pipe _bottomPipe;
    private bool _pipeCrossed;
    private bool _pipeScored;

    public Pipes(int screenWidth, float playableBackgroundHeight, int speed)
    {
      _speed = speed;
      _pipeCrossed = false;
      _pipeScored = false;
      int minimumPipeHeight = (int)(playableBackgroundHeight * PLAYABLE_BACKGROUND_HEIGHT_PERCENTAGE_FOR_MINIMUM_HEIGHT);
      int maximumPipeHeight = (int)(playableBackgroundHeight * PLAYABLE_BACKGROUND_HEIGHT_PERCENTAGE_FOR_MAXIMUM_HEIGHT) 
        - VERTICAL_GAP_BETWEEN_PIPES;
      int height = GenerateRandomNumber(minimumPipeHeight, maximumPipeHeight);
      _topPipe = new Pipe(screenWidth, TOP_PIPE_POSITION_Y, WIDTH, height);

      height = (int)playableBackgroundHeight - _topPipe.Height - VERTICAL_GAP_BETWEEN_PIPES;
      int y = (int)playableBackgroundHeight - height;
      _bottomPipe = new Pipe(screenWidth, y, WIDTH, height);
    }

    public void Display(Graphics graphics)
    {
      graphics.DrawImage(Properties.Resources.top_pipe, _topPipe.X, _topPipe.Y, _topPipe.Width, _topPipe.Height);
      graphics.DrawImage(Properties.Resources.bottom_pipe, _bottomPipe.X, _bottomPipe.Y, _bottomPipe.Width, _bottomPipe.Height);
    }

    public void Move()
    {
      if (_pipeCrossed && !_pipeScored)
      {
        _pipeScored = true;
      }
 
      _topPipe.X -= _speed; ;
      _bottomPipe.X = _topPipe.X;
    }

    public bool Collided(Bird bird)
    {
      /* return (X <= bird.X + bird.Width && X + Width >= bird.X &&
         (_bottomPipe.Y <= bird.Y + bird.Height || Height >= bird.Y)); */
      return _topPipe.Collided(bird) || _bottomPipe.Collided(bird);
    }

    public int GenerateRandomNumber(int min, int max)
    {
      Random random = new Random();
      return random.Next(min, max);
    }

    public void CheckPipeCrossed(Bird bird)
    {
      if (!_pipeCrossed)
      {
        if (_topPipe.X + WIDTH <= bird.X)
        {
          _pipeCrossed = true;
        }
      }
    }

    public bool OutOfScreenBirdCrossedPipe(Bird bird)
    {
      return bird.Y <= 0 && _topPipe.X - bird.Width <= bird.X && !_pipeCrossed;
    }

    public bool OutOfScreen()
    {
      return _topPipe.X + _topPipe.Width <= 0;
    }

    public bool PipeCrossed
    {
      get { return _pipeCrossed; }
    }

    public bool PipeScored
    {
      get { return _pipeScored; }
    }
  }
}
