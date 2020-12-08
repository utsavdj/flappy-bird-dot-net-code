using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FlappyBird
{
  class Ground : Sprite
  {
    private const int POSITION_X = 0;
    private const float SCREEN_HEIGHT_PERCENTAGE_FOR_POSITION_Y = 0.75f;
    private const float SCREEN_HEIGHT_PERCENTAGE_FOR_HEIGHT = 0.25f;

    private int _screenWidth;
    private int _speed;

    public Ground(int screenWidth, int screenHeight, int speed) : 
      base(POSITION_X, (int)(screenHeight * SCREEN_HEIGHT_PERCENTAGE_FOR_POSITION_Y), 
        2 * screenWidth, (int)(screenHeight * SCREEN_HEIGHT_PERCENTAGE_FOR_HEIGHT))
    {
      _screenWidth = screenWidth;
      _speed = speed;
    }

    public override void Display(Graphics graphics)
    {
      graphics.DrawImage(Properties.Resources.ground, X, Y, Width, Height);
    }

    public override void Move()
    {
      if (X <= -_screenWidth + _speed)
      {
        X = 0;
      }
      X -= _speed;
    }

  }
}
