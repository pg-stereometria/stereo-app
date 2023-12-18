using System.Collections.Generic;
using UnityEngine;

namespace StereoApp.Presenter.Base
{
    public abstract class FigurePresenter : MonoBehaviour
    {
        private readonly List<GameObject> _gameObjects = new();

        // Unity's .NET does not currently support covariant return types
        private object _figureObj;
        public virtual object FigureObj
        {
            get => _figureObj;
            set
            {
                _figureObj = value;
                OnChange();
            }
        }

        protected virtual void Start()
        {
            OnChange();
        }

        protected virtual void OnDestroy()
        {
            FigureObj = null;
            CleanupTrackedGameObjects();
        }

        protected virtual void OnChange()
        {
            CleanupTrackedGameObjects();
        }

        protected void CleanupTrackedGameObjects()
        {
            foreach (var gameObj in _gameObjects)
            {
                Destroy(gameObj);
            }

            _gameObjects.Clear();
        }

        protected void TrackGameObject(GameObject gameObj)
        {
            _gameObjects.Add(gameObj);
        }
    }

    public abstract class FigurePresenter<TFigure> : FigurePresenter
        where TFigure : class
    {
        public virtual TFigure Figure
        {
            get => (TFigure)base.FigureObj;
            set
            {
                base.FigureObj = value;
                OnChange();
            }
        }

        protected override void OnDestroy()
        {
            Figure = null; // trigger cleanup of i.e. property changed handlers
            base.OnDestroy();
        }
    }
}
