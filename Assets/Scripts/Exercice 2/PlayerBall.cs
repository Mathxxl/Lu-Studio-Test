using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBall : Ball
{
    #region Attributes

    #region Behaviour Booleans

    private bool _selected = false;
    private bool _charging = false;
    private bool _ischarging = false;

    #endregion
    
    
    private float _shotAngle = 0;
    private Slider _powerSlider;
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject powerSliderObject;

    #endregion
    
    #region Properties

    public float ShotAngle
    {
        get => _shotAngle;
        set => _shotAngle = value;
    }

    public bool Selected => _selected;
    
    #endregion

    #region Methods
    
    #region Private Methods

    /// <summary>
    /// Methods that add a force to the player ball depending on the angle and the force
    /// </summary>
    /// <param name="angle">Angle of the shoots in degrees</param>
    /// <param name="power"></param>
    private void Shoot(float angle, float power)
    {
        //Force Management
        angle *= Mathf.Deg2Rad;
        Vector2 force = new Vector2(-Mathf.Cos(angle), Mathf.Sin(angle)) * power * 400; // force = (cos(a), sin(a)) * power * compensation_factor ; the factor is arbitrary
        gameObject.GetComponent<Rigidbody2D>().AddForce(force);
        
        //Reset the values
        _selected = false;
        _charging = false;
        ui.SetActive(false);
    }

    /// <summary>
    /// Coroutine that manage the shoot depending on how long the player holds the click
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChargeShoot()
    {
        float val = 0f;
        powerSliderObject.SetActive(true);
        while (!Input.GetKeyUp(KeyCode.Space) && val <= 1)
        {
            yield return null;
            val += Time.deltaTime;
            _powerSlider.value = val;
        }
        powerSliderObject.SetActive(false);
        Shoot(_shotAngle, val);
        _charging = false;
    }

    /// <summary>
    /// Coroutine that ensures there is a delay between the selection of the ball and the charge of the shot
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator IsCharging(float time)
    {
        _ischarging = true;
        yield return new WaitForSeconds(time);
        _ischarging = false;
    }

    #endregion

    #region MonoBehaviours Handling

    protected override void Start()
    {
        ui.SetActive(false);
        _powerSlider = powerSliderObject.GetComponent<Slider>();
        Color = BallColor.White;
        base.Start();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _selected = !_selected;
            ui.SetActive(_selected);
            if(_selected) StartCoroutine(IsCharging(0.2f));
        }
    }

    private void Update()
    {
        if (_selected)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_charging && !_ischarging)
            {
                _charging = true;
                StartCoroutine(ChargeShoot());
            }
        }
    }

    #endregion

    #endregion
    
}
