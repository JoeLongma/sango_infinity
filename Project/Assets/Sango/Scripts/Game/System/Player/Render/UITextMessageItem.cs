using Sango.Game.Player;
using Sango.Loader;
using Sango.Render;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{

    public class UITextMessageItem : MonoBehaviour
    {
        public LayoutElement layoutElement;
        public RectTransform root;
        public Text contentText;
        public Text timeText;
        public Image forceImg;
        public PlayerMessage.TextMessage textMessage;
        public Action<PlayerMessage.TextMessage> onClickItem;

        public UITextMessageItem SetHeight(float height)
        {
            layoutElement.preferredHeight = 42;
            Vector2 size = root.sizeDelta;
            size.y = height;
            root.sizeDelta = size;
            return this;
        }

        public UITextMessageItem SetDate(int year, int month, int day)
        {
            if (year > 0)
            {
                timeText.text = $"《{year}年{month}月{day}日》";
            }
            else
            {
                timeText.text = "";
            }
            return this;
        }

        public UITextMessageItem SetForce(Force force)
        {
            forceImg.color = force != null ? force.Color : Color.white;
            return this;
        }

        public UITextMessageItem SetContent(string text)
        {
            contentText.text = text;
            return this;
        }

        public void OnClick()
        {
            onClickItem?.Invoke(textMessage);
        }
    }
}
