using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FlappyBird
{
  class Coin : Sprite
  {
    private const float POSITION_Y_OFFSET_PERCENTAGE = 0.07f;
    private const int NUMBER_OF_COINS = 3;
    private const int PIPE_HORIZONTAL_GAP = 331; //Calculated by checking the X position of current and previous pipe and pipe width by adding breakpoint

    private int _speed;
    private CoinType[] _coins;
    private int _score;

    private enum CoinType
    {
      BRONZE, SILVER, GOLD
    }
    private CoinType _coinType;

    public Coin(int screenWidth, int screenHeight, int speed) :
      base(screenWidth + (PIPE_HORIZONTAL_GAP - Properties.Resources.bronze_coin.Width) / 2, 0,
        Properties.Resources.bronze_coin.Width, Properties.Resources.bronze_coin.Height)
    {
      _speed = speed;
      int minCoinPositionY = (int)(screenHeight * POSITION_Y_OFFSET_PERCENTAGE);
      int maxCoinPositionY = screenHeight - minCoinPositionY - Width;
      Y = GenerateRandomNumber(minCoinPositionY, maxCoinPositionY + 1);
      _coins = new CoinType[NUMBER_OF_COINS];
      _coins[0] = CoinType.BRONZE;
      _coins[1] = CoinType.SILVER;
      _coins[2] = CoinType.GOLD;
      int minNumberOfCoins = 0;
      int maxNumberOfCoins = NUMBER_OF_COINS;
      int randomCoinIndex = GenerateRandomNumber(minNumberOfCoins, maxNumberOfCoins);
      _coinType = _coins[randomCoinIndex];
    }

    public override void Display(Graphics graphics)
    {
      Bitmap coinImage;
      if (_coinType == CoinType.GOLD)
      {
        coinImage = Properties.Resources.gold_coin;
        _score = 10;
      }
      else if (_coinType == CoinType.SILVER)
      {
        coinImage = Properties.Resources.silver_coin;
        _score = 5;
      }
      else
      {
        coinImage = Properties.Resources.bronze_coin;
        _score = 2;
      }
      graphics.DrawImage(coinImage, X, Y, Width, Height);
    }

    public override void Move()
    {
      X -= _speed;
    }

    public int Score
    {
      get { return _score; }
    }
  }
}
