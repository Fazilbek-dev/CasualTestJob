using ButchersGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlay : MonoBehaviour
{
    [SerializeField] private GameObject _touchText;
    [SerializeField] private GameObject _scoreText;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Animator _animator;


    private void Awake()
    {
        _playerController.enabled = false;
        _touchText.SetActive(true);
        _scoreText.SetActive(false);
    }
    private void Update()
    {
        if(Input.touchCount > 0)
        {
            _playerController.enabled = true;
            _touchText.SetActive(false);
            _scoreText.SetActive(true);
            _animator.SetBool("Start", true);
            this.gameObject.SetActive(false);
        }
    }
}
