using System.Collections;
using UnityEngine;

public class GalleryItem : MonoBehaviour {

    [SerializeField]
    private bool m_MustDiseapear = false;
    private bool m_Visible;
    private Animation m_Animation;
    private GameObject m_Parent;
    private bool m_IHaveAnimation;
    private bool m_CanBeHitted;
    private SGallery m_Gallery;
    public void SetGallery(SGallery g) { m_Gallery = g; }

    [SerializeField]
    private int m_Points = 5;

    private void OnEnable() {
        SGallery.OnGalleryCompleted += (int i) => { Reset(); };
    }

    private void OnDisable() {
        SGallery.OnGalleryCompleted -= (int i) => { Reset(); };
    }

    private void Awake() {
        m_Parent = this.transform.parent.gameObject;
        m_Animation = m_Parent.GetComponent<Animation>();
        m_IHaveAnimation = m_Animation != null;
    }

    private void Start() {
        Reset();
    }

    public void Hit() {
        if (m_IHaveAnimation) {
            m_Animation.Play(m_Parent.name + "_HIDE");
            StartCoroutine(CanBeVisibleAgain());
        }


        if (m_CanBeHitted) {
            m_Gallery.AddPoints(m_Points);
        }

        if (m_MustDiseapear) {
            uCore.Particles.PlayParticlesOnce("Hit_02", this.transform.position);
            this.transform.parent.gameObject.SetActive(false);
        }


        m_CanBeHitted = false;
    }

    private IEnumerator CanBeVisibleAgain() {
        yield return new WaitForSeconds(m_Animation.clip.length);
        m_Visible = false;
    }

    private IEnumerator CanBeHitted() {
        yield return new WaitForSeconds(m_Animation.clip.length);
        m_CanBeHitted = true;
    }

    public void Show() {
        if ((!m_IHaveAnimation) || (m_Visible))
            return;

        m_Visible = true;
        StartCoroutine(CanBeHitted());
        m_Animation.Play(m_Parent.name + "_SHOW");
    }

    private void Reset() {
        m_CanBeHitted = true;
        m_Visible = false;

        if (m_IHaveAnimation)
            m_Animation.Play(m_Parent.name + "_HIDE");
    }

}
