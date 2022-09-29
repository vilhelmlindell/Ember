using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FontStashSharp;
using Ember.Graphics;

namespace Ember.UI
{
    public enum TextAlignment
    {
        Left, 
        Center,
        Right
    }

    public enum FormatOption
    {
        None,
        TextToBounds,
        BoundsToText
    };
    
    public class TextBox : Control
    {
        public Vector2 Scale = Vector2.One;
        public Color Color = Color.White;
        public Color[] CharacterColors;
        public float Rotation;
        public bool UsesCharacterColors = false;
        public FormatOption FormatOption = FormatOption.BoundsToText;
        
        private byte[] _font;
        private int _fontSize;
        private bool _shouldFormatTextBox = false;
        private int _characterSpacing;
        private int _lineSpacing;
        private string _text;
        private string _textToDraw;
        private FontSystem _fontSystem = new FontSystem();
        private FontSystemSettings _fontSettings;

        public TextBox(byte[] font, 
                       string text = "", 
                       int fontSize = 10,
                       int characterSpacing = 0, 
                       int lineSpacing = 0) 
        {
            Font = font;
            Text = text;
            FontSize = fontSize;
            CharacterSpacing = characterSpacing;
            LineSpacing = lineSpacing;
            Resized += () => _shouldFormatTextBox = true;
        }

        public byte[] Font
        {
            get => _font;
            set
            {
                _font = value;
                _fontSystem.Reset();
                _fontSystem.AddFont(_font);
                _shouldFormatTextBox = true;
            }
        } 
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                _textToDraw = value;
                _shouldFormatTextBox = true;
            }
        }
        public int FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                _shouldFormatTextBox = true;
            }
        }
        public int CharacterSpacing
        {
            get => _characterSpacing;
            set
            {
                _characterSpacing = value;
                _shouldFormatTextBox = true;
            }
        }
        public int LineSpacing
        {
            get => _lineSpacing;
            set
            {
                _lineSpacing = value;
                _shouldFormatTextBox = true;
            }
        }
        public FontSystemSettings FontSettings
        {
            get => _fontSettings;
            set
            {
                _fontSettings = value;
                _fontSystem = new FontSystem(value);
                _shouldFormatTextBox = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_shouldFormatTextBox)
            {
                FormatTextBox();
                _shouldFormatTextBox = false;
            }
        }

        public override void Draw(GraphicsContext graphicsContext, GameTime gameTime)
        {
            if (UsesCharacterColors)
            {
                graphicsContext.SpriteBatch.DrawString(_fontSystem.GetFont(FontSize), _textToDraw, AbsolutePosition, CharacterColors, Scale,
                    Rotation, Vector2.Zero, LayerDepth, CharacterSpacing, LineSpacing);
            }
            else
            {
                Console.WriteLine("Test");
                graphicsContext.SpriteBatch.DrawString(_fontSystem.GetFont(FontSize), _textToDraw, AbsolutePosition, Color, Scale,
                    Rotation, Vector2.Zero, LayerDepth, CharacterSpacing, LineSpacing);
            }
        }

        private void FormatTextBox()
        {
            switch (FormatOption)
            {
                case FormatOption.TextToBounds:
                    FormatTextToBounds();
                    break;
                case FormatOption.BoundsToText:
                    FormatBoundsToText();
                    break;
                case FormatOption.None:
                    break;
            }
        }
        private void FormatTextToBounds()
        {
            if (_fontSystem.GetFont(FontSize).MeasureString(_text, Scale, CharacterSpacing, LineSpacing).X > ActualWidth)
            {
                List<string> lines = new List<string>();
                string lineText = _text;
                bool endOfLineReached = false;
                while (_fontSystem.GetFont(FontSize).MeasureString(lineText, Scale, CharacterSpacing, LineSpacing).X > ActualWidth)
                {
                    for (int i = 1; i <= lineText.Length; i++)
                    {
                        if (i == lineText.Length)
                        {
                            lines.Add(lineText);
                            endOfLineReached = true;
                            break;
                        }

                        if (_fontSystem.GetFont(FontSize).MeasureString(lineText[..i], Scale, CharacterSpacing, LineSpacing).X > ActualWidth) 
                        {
                            lines.Add(lineText[..i] + "\n");
                            lineText = lineText[i..];
                            break;
                        }
                    }
                    if (endOfLineReached)
                        break;
                }
                _textToDraw = string.Concat(lines);
            }
            else
                _textToDraw = _text;
        }

        private void FormatBoundsToText()
        {
            Width.Unit = LengthUnit.Pixels;
            Height.Unit = LengthUnit.Pixels;
            Vector2 size = _fontSystem.GetFont(FontSize).MeasureString(_text, Scale, CharacterSpacing, LineSpacing);
            Width.Value = size.X;
            Height.Value = size.Y;
        }
    }
}
