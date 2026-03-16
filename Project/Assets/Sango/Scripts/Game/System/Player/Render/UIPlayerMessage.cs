using Sango.Game.Player;
using Sango.Loader;
using Sango.Render;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{

    public class UIPlayerMessage : UGUIWindow
    {
        public GameObject root;
        public UITextMessageItem textMessageItem;
        public UIPersonMessageItem personMessageItem;

        public LoopVerticalScrollRectMulti textMessageScrollRect;
        public LoopVerticalScrollRectMulti personMessageScrollRect;

        CreatePool<UITextMessageItem> textMsgItemPool;
        CreatePool<UIPersonMessageItem> personMsgItemPool;
        TextMessageScroll textMessageScroll;
        PersonMessageScroll personMessageScroll;

        PlayerMessage playerMessage;

        class TextMessageScroll : LoopScrollPrefabSource, LoopScrollMultiDataSource
        {
            internal UIPlayerMessage uIPlayerMessage;
            internal CreatePool<UITextMessageItem> pool;
            public GameObject GetObject(int index)
            {
                UITextMessageItem uITextMessageItem = pool.Create();
                uITextMessageItem.onClickItem = uIPlayerMessage.OnClickTextMessage;
                return uITextMessageItem.gameObject;
            }

            public void ReturnObject(Transform trans)
            {
                UITextMessageItem uITextMessageItem = trans.GetComponent<UITextMessageItem>();
                if (uITextMessageItem != null)
                    pool.Recycle(uITextMessageItem);
            }

            public void ProvideData(Transform transform, int idx)
            {
                if (idx < 0 || idx >= uIPlayerMessage.playerMessage.textMessages.Count)
                {
                    transform.gameObject.SetActive(false);
                    return;
                }
                if (!transform.gameObject.activeSelf)
                    transform.gameObject.SetActive(true);
                UITextMessageItem uITextMessageItem = transform.GetComponent<UITextMessageItem>();
                PlayerMessage.TextMessage data = uIPlayerMessage.playerMessage.textMessages[idx];
                uITextMessageItem.SetDate(data.year, data.month, data.day).SetForce(data.force).SetContent(data.text);
                uITextMessageItem.SetHeight(data.year > 0 ? 42 : 24);
                uITextMessageItem.textMessage = data;
            }
        }

        class PersonMessageScroll : LoopScrollPrefabSource, LoopScrollMultiDataSource
        {
            internal UIPlayerMessage uIPlayerMessage;
            internal CreatePool<UIPersonMessageItem> pool;

            public GameObject GetObject(int index)
            {
                if (index < 0 || index >= uIPlayerMessage.playerMessage.personMessages.Count)
                    return null;
                UIPersonMessageItem uITextMessageItem = pool.Create();
                return uITextMessageItem.gameObject;
            }

            public void ReturnObject(Transform trans)
            {
                UIPersonMessageItem uITextMessageItem = trans.GetComponent<UIPersonMessageItem>();
                if (uITextMessageItem != null)
                    pool.Recycle(uITextMessageItem);
            }

            public void ProvideData(Transform transform, int idx)
            {
                if (idx < 0 || idx >= uIPlayerMessage.playerMessage.personMessages.Count)
                    return;
                UIPersonMessageItem uITextMessageItem = transform.GetComponent<UIPersonMessageItem>();
                PlayerMessage.PersonMessage data = uIPlayerMessage.playerMessage.personMessages[idx];
                uITextMessageItem.SetData(data.text, data.person);
            }
        }

        protected override void Awake()
        {
            textMsgItemPool = new CreatePool<UITextMessageItem>(textMessageItem);
            personMsgItemPool = new CreatePool<UIPersonMessageItem>(personMessageItem);
            textMessageScroll = new TextMessageScroll() { uIPlayerMessage = this, pool = textMsgItemPool };
            personMessageScroll = new PersonMessageScroll() { uIPlayerMessage = this, pool = personMsgItemPool };

            textMessageScrollRect.prefabSource = textMessageScroll;
            textMessageScrollRect.dataSource = textMessageScroll;
            personMessageScrollRect.prefabSource = personMessageScroll;
            personMessageScrollRect.dataSource = personMessageScroll;

        }

        public override void OnShow()
        {
            base.OnShow();
            playerMessage = GameSystem.GetSystem<PlayerMessage>();

            textMessageScrollRect.totalCount = playerMessage.textMessages.Count;
            personMessageScrollRect.totalCount = playerMessage.personMessages.Count;

            playerMessage.onTextMessageAdd += OnTextMessageAdd;
            playerMessage.onPersonMessageAdd += OnPersonMessageAdd;

            playerMessage.onVisibleChange += OnMessagePlaneVisible;
        }

        void OnMessagePlaneVisible(bool visible)
        {
            root.SetActive(visible);
        }

        void OnTextMessageAdd(PlayerMessage.TextMessage msg, PlayerMessage message)
        {
            textMessageScrollRect.totalCount = message.textMessages.Count;
            textMessageScrollRect.RefillCellsFromEnd();
        }

        void OnPersonMessageAdd(PlayerMessage.PersonMessage msg, PlayerMessage message)
        {
            personMessageScrollRect.totalCount = message.personMessages.Count;
            textMessageScrollRect.RefillCellsFromEnd();
        }

        public void OnMinButton()
        {
            playerMessage.onVisibleChange?.Invoke(false);
        }

        void OnClickTextMessage(PlayerMessage.TextMessage msg)
        {
            if (msg != null)
            {
                Cell cell = Scenario.Cur.Map.GetCell(msg.x, msg.y);
                if (cell != null)
                    MapRender.Instance.MoveCameraTo(cell.Position, 0.3f);
            }
        }
    }
}
