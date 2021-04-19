using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.VisualBasic.CompilerServices;

namespace Tetris
{
    public class Menu
    {
        Choice[] currentChoices;
        Choice[] mainChoices;
        Choice[] controlChoices;
        Choice[] quitChoices;
        Choice[] subscreenChoices;
        Choice[] lossChoices;
        float[] defaultPositionsY;
        float[] defaultPositionX;
        public Keys drop { get; set; }
        public Keys rotLeft { get; set; }
        public Keys rotRight { get; set; }
        public Keys hardDrop { get; set; }
        public Keys moveLeft { get; set; }
        public Keys moveRight { get; set; }

        public Game1.SCREEN destinationScreen { get; set; }
        Game1.SCREEN curScreen;
        int screenWidth;
        int screenHeight;
        int iconPosition;
        string title;
        bool stopFunction;

        Texture2D iconTexture;
        SpriteFont menuFont;
        SpriteFont titleFont;

        public Menu(ContentManager content, int width, int height, Keys d, Keys rr, Keys rl, Keys mr, Keys ml, Keys hd)
        {
            screenWidth = width;
            screenHeight = height;
            iconPosition = 0;
            curScreen = Game1.SCREEN.MAIN;
            title = "MAIN MENU";
            stopFunction = false;
            drop = d;
            rotRight = rr;
            rotLeft = rl;
            moveLeft = ml;
            moveRight = mr;
            hardDrop = hd;

            destinationScreen = Game1.SCREEN.MAIN;

            
            iconTexture = content.Load<Texture2D>("icon");
            menuFont = content.Load<SpriteFont>("menuFont");
            titleFont = content.Load<SpriteFont>("titleFont");

            defaultPositionsY = new float[10] { screenHeight * 0.27f, screenHeight * 0.35f, screenHeight * 0.40f, screenHeight * 0.45f, screenHeight * 0.50f, screenHeight * 0.55f, screenHeight * 0.60f, screenHeight * 0.65f, screenHeight * 0.70f, screenHeight * 0.75f };
            defaultPositionX = new float[2] { screenWidth * 0.37f, screenWidth * 0.40f };
            mainChoices = new Choice[5]  {
                new Choice(Game1.ACTION.PLAY, new Vector2(defaultPositionX[1], defaultPositionsY[1]), "PLAY", Game1.SCREEN.PLAY),
                new Choice(Game1.ACTION.HIGH_SCORES, new Vector2(defaultPositionX[1], defaultPositionsY[2]), "HIGH SCORES", Game1.SCREEN.HIGH_SCORES),
                new Choice(Game1.ACTION.CONTROLS, new Vector2(defaultPositionX[1], defaultPositionsY[3]), "CONTROLS", Game1.SCREEN.CONTROLS),
                new Choice(Game1.ACTION.CREDITS, new Vector2(defaultPositionX[1], defaultPositionsY[4]), "CREDITS", Game1.SCREEN.CREDITS),
                new Choice(Game1.ACTION.QUIT, new Vector2(defaultPositionX[1], defaultPositionsY[5]), "QUIT")
            };

            controlChoices = new Choice[7]  {
                new Choice(Game1.ACTION.CHANGE_DROP, new Vector2(defaultPositionX[1], defaultPositionsY[1]),  "DROP               [ " + drop.ToString() + " ]"),
                new Choice(Game1.ACTION.CHANGE_HARDDROP, new Vector2(defaultPositionX[1], defaultPositionsY[2]),  "HARD DROP        [ " + hardDrop.ToString() + " ]"),
                new Choice(Game1.ACTION.CHANGE_ROTLEFT, new Vector2(defaultPositionX[1], defaultPositionsY[3]),  "ROTATE LEFT      [ " + rotLeft.ToString() + " ]"),
                new Choice(Game1.ACTION.CHANGE_ROTRIGHT, new Vector2(defaultPositionX[1], defaultPositionsY[4]), "ROTATE RIGHT  [ " + rotRight.ToString() + " ]"),
                new Choice(Game1.ACTION.CHANGE_MOVELEFT, new Vector2(defaultPositionX[1], defaultPositionsY[5]),  "MOVE LEFT      [ " + rotLeft.ToString() + " ]"),
                new Choice(Game1.ACTION.CHANGE_MOVERIGHT, new Vector2(defaultPositionX[1], defaultPositionsY[6]), "MOVE RIGHT  [ " + rotRight.ToString() + " ]"),
                new Choice(Game1.ACTION.BACK, new Vector2(defaultPositionX[1], defaultPositionsY[7]), "BACK", Game1.SCREEN.MAIN)
            };

            subscreenChoices = new Choice[1]  {
                new Choice(Game1.ACTION.BACK, new Vector2(defaultPositionX[1], defaultPositionsY[1]), "BACK", Game1.SCREEN.MAIN)
            };

            quitChoices = new Choice[2]  {
                new Choice(Game1.ACTION.CONTINUE, new Vector2(defaultPositionX[1], defaultPositionsY[1]), "CONTINUE"),
                new Choice(Game1.ACTION.QUIT, new Vector2(defaultPositionX[1], defaultPositionsY[2]), "QUIT"),
            };

            lossChoices = new Choice[2]
            {
                new Choice(Game1.ACTION.PLAY, new Vector2(defaultPositionX[1], defaultPositionsY[1]), "TRY AGAIN?", Game1.SCREEN.PLAY),
                new Choice(Game1.ACTION.BACK, new Vector2(defaultPositionX[1], defaultPositionsY[2]), "RETURN TO MAIN MENU", Game1.SCREEN.MAIN)
            };

            currentChoices = mainChoices;
        }

        public void updateDrop(Keys key)
        {
            drop = key;
            controlChoices[0] = new Choice(Game1.ACTION.CHANGE_DROP, new Vector2(defaultPositionX[1], defaultPositionsY[1]), "DROP               [ " + drop.ToString() + " ]");
        }
        public void updateRotLeft(Keys key)
        {
            rotLeft = key;
            controlChoices[2] = new Choice(Game1.ACTION.CHANGE_ROTLEFT, new Vector2(defaultPositionX[1], defaultPositionsY[2]), "ROTATE LEFT      [ " + rotLeft.ToString() + " ]");
        }
        public void updateRotRight(Keys key)
        {
            rotRight = key;
            controlChoices[3] = new Choice(Game1.ACTION.CHANGE_ROTRIGHT, new Vector2(defaultPositionX[1], defaultPositionsY[3]), "ROTATE RIGHT  [ " + rotRight.ToString() + " ]");
        }
        public void updateHardDrop(Keys key)
        {
            drop = key;
            controlChoices[1] = new Choice(Game1.ACTION.CHANGE_HARDDROP, new Vector2(defaultPositionX[1], defaultPositionsY[1]), "HARD DROP               [ " + hardDrop.ToString() + " ]");
        }
        public void updateMoveLeft(Keys key)
        {
            rotLeft = key;
            controlChoices[4] = new Choice(Game1.ACTION.CHANGE_MOVELEFT, new Vector2(defaultPositionX[1], defaultPositionsY[2]), "MOVE LEFT      [ " + moveLeft.ToString() + " ]");
        }
        public void updateMoveRight(Keys key)
        {
            rotRight = key;
            controlChoices[5] = new Choice(Game1.ACTION.CHANGE_MOVERIGHT, new Vector2(defaultPositionX[1], defaultPositionsY[3]), "MOVE RIGHT  [ " + moveRight.ToString() + " ]");
        }

        public Game1.ACTION Update(Game1.SCREEN scr)
        {
            Game1.ACTION action = Game1.ACTION.NULL;
            bool preventInput = false;
            if (curScreen != scr)
            {
                preventInput = true;
                iconPosition = 0;
                curScreen = scr;
                switch (scr)
                {
                    case Game1.SCREEN.MAIN:
                        title = "MAIN MENU";
                        currentChoices = mainChoices;
                        break;
                    case Game1.SCREEN.HIGH_SCORES:
                        title = "HIGH SCORES";
                        currentChoices = subscreenChoices;
                        break;
                    case Game1.SCREEN.CONTROLS:
                        title = "CONTROL CONFIGURATION";
                        currentChoices = controlChoices;
                        break;
                    case Game1.SCREEN.CREDITS:
                        title = "CREDITS";
                        currentChoices = subscreenChoices;
                        break;
                    case Game1.SCREEN.PLAY:
                        title = "";
                        currentChoices = new Choice[0];
                        break;
                    case Game1.SCREEN.QUIT:
                        title = "ARE YOU SURE YOU WANT TO QUIT?";
                        currentChoices = quitChoices;
                        break;
                    case Game1.SCREEN.LOSE:
                        title = "MISSION FAILED";
                        currentChoices = lossChoices;
                        break;
                }
            }
            else
            {
                preventInput = false;
            }

            if (!preventInput && !stopFunction)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    action = Game1.ACTION.QUIT;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    action = currentChoices[iconPosition].action;
                    stopFunction = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    iconPosition += (iconPosition + 1) > currentChoices.Length - 1 ? 0 : 1;
                    stopFunction = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    iconPosition -= (iconPosition - 1) < 0 ? 0 : 1;
                    stopFunction = true;
                }
            }
            if (Keyboard.GetState().GetPressedKeyCount() == 0)
            {
                stopFunction = false;
            }

            return action;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            

            switch (curScreen)
            {
                case Game1.SCREEN.MAIN:
                case Game1.SCREEN.LOSE:
                case Game1.SCREEN.HIGH_SCORES:
                case Game1.SCREEN.CREDITS:
                case Game1.SCREEN.CONTROLS:
                case Game1.SCREEN.QUIT:
                    if (currentChoices.Length > 0)
                        spriteBatch.Draw(iconTexture, new Rectangle((int)defaultPositionX[1] - (int)(screenHeight * 0.04), (int)currentChoices[iconPosition].position.Y + 20, (int)(screenWidth * 0.02f), (int)(screenWidth * 0.02f)), Color.White);
                    spriteBatch.DrawString(titleFont, title, new Vector2(defaultPositionX[0], defaultPositionsY[0]), Color.White);
                    foreach (Choice choice in currentChoices)
                    {
                        spriteBatch.DrawString(menuFont, choice.key, choice.position, Color.White);
                    }

                    if (curScreen == Game1.SCREEN.HIGH_SCORES)
                    {
                        spriteBatch.DrawString(menuFont, "1. " + Game1.HighScores[0].ToString(), new Vector2(defaultPositionX[1], defaultPositionsY[3]), Color.White);
                        spriteBatch.DrawString(menuFont, "2. " + Game1.HighScores[1].ToString(), new Vector2(defaultPositionX[1], defaultPositionsY[4]), Color.White);
                        spriteBatch.DrawString(menuFont, "3. " + Game1.HighScores[2].ToString(), new Vector2(defaultPositionX[1], defaultPositionsY[5]), Color.White);
                        spriteBatch.DrawString(menuFont, "4. " + Game1.HighScores[3].ToString(), new Vector2(defaultPositionX[1], defaultPositionsY[6]), Color.White);
                        spriteBatch.DrawString(menuFont, "5. " + Game1.HighScores[4].ToString(), new Vector2(defaultPositionX[1], defaultPositionsY[7]), Color.White);
                    }
                    if (curScreen == Game1.SCREEN.CREDITS)
                    {
                        spriteBatch.DrawString(menuFont, "Game Developed By: ", new Vector2(defaultPositionX[1], defaultPositionsY[3]), Color.White);
                        spriteBatch.DrawString(menuFont, "Abraham Gunther", new Vector2(defaultPositionX[1], defaultPositionsY[4]), Color.White);
                    }
                    if (curScreen == Game1.SCREEN.LOSE)
                    {
                        spriteBatch.DrawString(menuFont, "Your Score: ", new Vector2(defaultPositionX[1] - (int)(screenHeight * 0.1), defaultPositionsY[4]), Color.White);
                    }
                    break;
            }
            spriteBatch.End();
        }
    }
    class Choice
    {
        public Game1.ACTION action;
        public Vector2 position;
        public string key;

        public Choice(Game1.ACTION act, Vector2 pos, string k, Game1.SCREEN destination = Game1.SCREEN.NULL)
        {
            action = act;
            position = pos;
            key = k;
        }
    }
}
