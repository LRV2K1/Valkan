using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Audio;

public class TextWriter : TextGameObject
{
    TextBox textBox;

    protected List<string> textLines;
    protected string tempLetter;

    protected int currentLine;
    protected int startAtLine;
    protected int endAtLine;

    protected bool isTyping = false;
    protected bool isActive = false;

    protected double typeSpeed;
    protected double tempTypeSpeed;
    protected double time;

    protected bool isWaiting;
    protected bool isSpecial;

    protected int letter = 0;
    protected int length;
    protected string startletter;
    protected string temptext;
    protected string lineOfText;

    protected string textType;

    public TextWriter(string character, string textfile, string type)
        : base("Fonts/verdana",100,"textwriter")
    {
        textBox = new TextBox();
        textBox.Parent = this;
        textType = type;
        this.Position = new Vector2(GameEnvironment.Screen.X/8, GameEnvironment.Screen.Y * 0.7f);
        this.Visible = true;
        isSpecial = false;
        LoadText("Content/Dialogue/" + character + "/" + textfile + ".txt");
        time = 0;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.IsKeyDown(Keys.Space))
        {
            if (!isTyping)
            {
                typeSpeed = 0.024f;
            }
            else if (isTyping)
            {
                typeSpeed = 0.005f;
            }
        }
        else if(inputHelper.KeyPressed(Keys.R))
        {
            if (!isActive && currentLine < endAtLine)
                EnableTextBox();
            else if (!isActive && currentLine >= endAtLine)
            {
                DisableTextBox();
                currentLine = 0;
            }

        }
    }

    public override void Update(GameTime gameTime)
    {
        if (!isActive)
        {
            return;
        }
        if (isTyping && lineOfText != null && (letter < lineOfText.Length))
        {
            if (isSpecial)
            {
                switch (startletter)
                {
                    case "s":
                        if(tempLetter != "}")
                        {
                            temptext += tempLetter;
                            letter++;
                            tempLetter = lineOfText[letter].ToString();
                        }
                        else
                        {
                            isSpecial = false;
                            letter++;
                            GameEnvironment.AssetManager.PlaySound("Sounds/" + temptext);
                        }
                        break;
                    case "{":
                        if(tempLetter != "}")
                        {
                            startletter = tempLetter;
                            this.Color = Color.Red;
                        }
                        else
                        {
                            isSpecial = false;
                            letter++;
                        }
                        break;
                    case "(":
                        if (tempLetter != ")")
                        {
                            letter++;
                            tempLetter = lineOfText[letter].ToString();
                        }
                        else
                        {
                            isSpecial = false;
                            letter++;
                        }
                        break;
                    case "<":
                        break;
                }
            }
            else if (time <= 0)
            {
                time += typeSpeed;
                tempLetter = lineOfText[letter].ToString();
                switch (tempLetter)
                {
                    case ".":
                        typeSpeed = 0.24f;
                        this.Text += lineOfText[letter];
                        GameEnvironment.AssetManager.PlaySound("Sounds/Textbeep");
                        letter++;
                        break;
                    case "{":
                    case "(":
                    case "<":
                        startletter = lineOfText[letter].ToString();
                        letter++;
                        tempLetter = lineOfText[letter].ToString();
                        isSpecial = true;
                        break;
                    default:
                        typeSpeed = 0.024f;
                        this.Text += lineOfText[letter];
                        GameEnvironment.AssetManager.PlaySound("Sounds/Textbeep");
                        letter++;
                        break;
                }
            }
            else
            {
                time -= gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        else
        {
            isTyping = false;
            isActive = false;
            if (!(currentLine > endAtLine))
            {
                currentLine++;
            }
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        textBox.Draw(gameTime, spriteBatch);
        base.Draw(gameTime, spriteBatch);
    }

    public void EnableTextBox()
    {
        letter = 0;
        this.Text = "";
        lineOfText = textLines[currentLine];
        isTyping = true;
        isActive = true;
        this.Visible = true;
        textBox.Visible = true;
    }

    public void DisableTextBox()
    {
        this.Visible = false;
        textBox.Visible = false;
    }

    public void LoadText(string path)
    {
        StreamReader streamReader = new StreamReader(path);
        textLines = new List<string>();
        string line = streamReader.ReadLine();
        while (line != null)
        {
            textLines.Add(line);
            line = streamReader.ReadLine();
            endAtLine++;
        }
        streamReader.Close();
        currentLine = 0;
    }

}
