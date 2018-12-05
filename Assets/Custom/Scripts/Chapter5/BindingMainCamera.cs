using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BindingMainCamera : MonoBehaviour {

    [SerializeField] private PlayableDirector director;

	void Awake () {
        director = GetComponent<PlayableDirector>();
        TimelineAsset asset = director.playableAsset as TimelineAsset;
        director.SetGenericBinding(asset.GetOutputTrack(0), Camera.main.gameObject);
    }
}
