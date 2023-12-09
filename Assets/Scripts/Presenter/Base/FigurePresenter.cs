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
        private readonly List<GameObject> _gameObjects = new();

        public virtual TFigure Figure
        {
            get => (TFigure)base.FigureObj;
            set
            {
                base.FigureObj = value;
                OnChange();
            }
        }
    }
}
