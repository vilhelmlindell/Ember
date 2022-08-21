using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FontStashSharp;
using Ember.Graphics;

namespace Ember.GUI
{
    public class TextBox : Widget
    {
        public float Rotation;
        public Vector2? Scale;
        public Color Color = Color.White;
        public Color[] CharacterColors;
        private FontSystem _fontSystem = new FontSystem();
        private FontSystemSettings _fontSettings;
        private byte[] _font;
        private int _fontSize;
        private int _characterSpacing;
        private int _lineSpacing;
        private string _text;
        private string _textToDraw;

        public TextBox(byte[] font = null, string text = "", int fontSize = 10, int characterSpacing = 0, int lineSpacing = 0)
        {
            if (font != null)
                Font = font;
            else
                Font = Fonts.OpenSans;
            _text = text;
            _fontSize = fontSize;
            _characterSpacing = characterSpacing;
            _lineSpacing = lineSpacing;
            FormatTextToBounds();
        }

        public byte[] Font
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
                _fontSystem.Reset();
                _fontSystem.AddFont(_font);
            }
        } 
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                FormatTextToBounds();
            }
        }
        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;
                FormatTextToBounds();
            }
        }
        public int CharacterSpacing
        {
            get { return _characterSpacing; }
            set
            {
                _characterSpacing = value;
                FormatTextToBounds();
            }
        }
        public int LineSpacing
        {
            get { return _lineSpacing; }
            set
            {
                _lineSpacing = value;
                FormatTextToBounds();
            }
        }
        public SpriteFontBase SpriteFont
        {
            get
            {
                return _fontSystem.GetFont(FontSize);
            }
        }
        public FontSystemSettings FontSettings
        {
            get { return _fontSettings; }
            set
            {
                _fontSettings = value;
                _fontSystem = new FontSystem(value);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (CharacterColors != null)
            {
                spriteBatch.DrawString(SpriteFont, _textToDraw, AbsolutePosition, CharacterColors, Scale,
                                       Rotation, Origin, LayerDepth, CharacterSpacing, LineSpacing);
            }
            else
            {
                spriteBatch.DrawString(SpriteFont, _textToDraw, AbsolutePosition, Color, Scale,
                                       Rotation, Origin, LayerDepth, CharacterSpacing, LineSpacing);
            }
        }

        public void FormatTextToBounds()
        {
            if (SpriteFont.MeasureString(_text, Scale, CharacterSpacing, LineSpacing).X > Width)
            {
                List<string> lines = new List<string>();
                string lineText = _text;
                bool endOfLineReached = false;
                while (SpriteFont.MeasureString(lineText, null, CharacterSpacing, LineSpacing).X > Height)
                {
                    for (int i = 1; i <= lineText.Length; i++)
                    {
                        if (i == lineText.Length)
                        {
                            lines.Add(lineText);
                            endOfLineReached = true;
                            break;
                        }

                        if (SpriteFont.MeasureString(lineText[..i], null, CharacterSpacing, LineSpacing).X > Width) 
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
    }
}
