using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FlappyBird
{
  class Bird : Sprite
  {
    private const float SCREEN_HEIGHT_PERCENTAGE_FOR_POSITION_Y = 0.75f;
    private const int INITIAL_JUMP_VELOCITY = 4;
    private const int GRAVITY = 1;
    private const int NUMBER_OF_BIRD_IMAGES = 4;
    private static readonly Bitmap DEFAULT_BIRD_IMAGE = Properties.Resources.bird_1;

    private int _fallVelocity;
    private int _jumpVelocity;
    private Bitmap[] _birdImages;
    private Bitmap _birdImage = DEFAULT_BIRD_IMAGE;
    private static readonly int WIDTH = DEFAULT_BIRD_IMAGE.Width;
    private static readonly int HEIGHT = DEFAULT_BIRD_IMAGE.Height;
    
    public Bird(int screenWidth, int screenHeight) : 
      base((screenWidth - WIDTH) / 2, 
        (int)(screenHeight * SCREEN_HEIGHT_PERCENTAGE_FOR_POSITION_Y - HEIGHT) / 2, 
        WIDTH, HEIGHT)
    {
      _birdImages = new Bitmap[NUMBER_OF_BIRD_IMAGES];
      _birdImages[0] = DEFAULT_BIRD_IMAGE;
      _birdImages[1] = Properties.Resources.bird_2;
      _birdImages[2] = Properties.Resources.bird_3;
      _birdImages[3] = Properties.Resources.bird_4;

      _fallVelocity = GRAVITY;
      _jumpVelocity = INITIAL_JUMP_VELOCITY;
    }

    public override void Display(Graphics graphics)
    {
      graphics.DrawImage(_birdImage, X, Y);
    }

    public override void Move()
    {
      Y -= _jumpVelocity;
      _jumpVelocity += GRAVITY;
      _fallVelocity = GRAVITY;
    }

    public void Fall()
    {
      Y += _fallVelocity;
      _fallVelocity += GRAVITY;
      _jumpVelocity = INITIAL_JUMP_VELOCITY;
    }

    public Bitmap GetBirdImage()
    {
      return _birdImage;
    }

    public void SetBirdImage(int index)
    {
      _birdImage = _birdImages[index];
    }

  }
}
