using System;
using System.Collections.Generic;
using UnityEngine;
using WaveProject.Services;
using WaveProject.UI.View;
using Object = UnityEngine.Object;

namespace WaveProject.UI
{
    internal class UIManager : IService
    {
        private readonly RoutineService _routineService;
        private readonly Canvas _canvas;

        private readonly Dictionary<Type, IView> _spawnedUiViews = new();

        private Dictionary<Type, AbstractView> _uiViewsPrefabs;

        private IView _currentView;
        private IView _currentPopupView;

        private IView _lastClosedUI;

        public UIManager(UIViews uiViews, Canvas canvas, Canvas bestiaryCanvas)
        {
            _uiViewsPrefabs = new Dictionary<Type, AbstractView>
            {
                { uiViews.BestiaryUiView.GetType(), uiViews.BestiaryUiView },
            };
            _canvas = canvas;
        }

        public void OpenUI(Type type, Action openedCallback = null, Action closedCallback = null)
        {
            _lastClosedUI = _currentView;
            _currentView?.Close(closedCallback);

            if (TryGetSpawnedView(type, out var view))
            {
                _currentView = view;
                view.Open(openedCallback);
            }
            else
            {
                if (TryGetNewView(type, out var abstractView))
                {
                    _currentView = abstractView;
                    _currentView.Open(openedCallback);
                }
                else
                    Debug.LogError($"{nameof(_uiViewsPrefabs)} not contains the prefab of type {type}." +
                                   $" Register it in constructor!");
            }
        }

        public void OpenLastUI(Action openedCallback = null, Action closedCallback = null)
        {
            _currentView?.Close(closedCallback);

            _currentView = _lastClosedUI;
            _lastClosedUI.Open(openedCallback);
        }

        public void OpenPopup(Type type)
        {
            if (_spawnedUiViews.TryGetValue(type, out var view))
            {
                _currentPopupView = view;
                view.Open();
            }
            else
            {
                if (_uiViewsPrefabs.TryGetValue(type, out var prefab))
                {
                    _currentPopupView = Object.Instantiate(prefab, _canvas.transform);
                    _currentPopupView.Init();
                    _spawnedUiViews.Add(_currentPopupView.GetType(), _currentPopupView);
                }
                else
                    Debug.LogError(
                        $"{nameof(_uiViewsPrefabs)} not contains the prefab of type {type}. Register it in constructor!");
            }
        }

        public void ClosePopup(Type type)
        {
        }

        public AbstractView GetViewByType(Type type)
        {
            if (TryGetSpawnedView(type, out var spawnedView)) return spawnedView;
            if (TryGetNewView(type, out var newView)) return newView;

            Debug.LogError(
                $"{nameof(_uiViewsPrefabs)} not contains the prefab of type {type}. Register it in constructor!");
            return null;
        }

        private bool TryGetNewView(Type type, out AbstractView abstractView)
        {
            if (_uiViewsPrefabs.TryGetValue(type, out var prefab))
            {
                var canvas = _canvas.transform;

                var newView = Object.Instantiate(prefab, canvas);
                newView.Close();
                newView.transform.SetAsFirstSibling();

                newView.Init();
                _spawnedUiViews.Add(newView.GetType(), newView);

                abstractView = newView;
                return true;
            }

            abstractView = null;
            return false;
        }

        private bool TryGetSpawnedView(Type type, out AbstractView abstractView)
        {
            if (_spawnedUiViews.TryGetValue(type, out var view))
            {
                abstractView = (AbstractView)view;
                return true;
            }

            abstractView = null;
            return false;
        }
    }
}