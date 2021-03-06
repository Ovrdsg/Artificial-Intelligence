using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleScripts
{
    public class MainWindowObserver : MonoBehaviour
    {
        [Header("Player Stats")]
        [SerializeField] private TMP_Text _countMoneyText;
        [SerializeField] private TMP_Text _countHealthText;
        [SerializeField] private TMP_Text _countPowerText;
        [SerializeField] private TMP_Text _countArmorText;

        [Header("Enemy Stats")]
        [SerializeField] private TMP_Text _countPowerEnemyText;

        [Header("Money Buttons")]
        [SerializeField] private Button _addMoneyButton;
        [SerializeField] private Button _minusMoneyButton;

        [Header("Health Buttons")]
        [SerializeField] private Button _addHealthButton;
        [SerializeField] private Button _minusHealthButton;

        [Header("Armor Buttons")]
        [SerializeField] private Button _addArmorButton;
        [SerializeField] private Button _minusArmorButton;

        [Header("Power Buttons")]
        [SerializeField] private Button _addPowerButton;
        [SerializeField] private Button _minusPowerButton;

        [Header("Other Buttons")]
        [SerializeField] private Button _fightButton;

        [Header("End Game Variants")]
        [SerializeField] GameObject _displayWonGame;
        [SerializeField] GameObject _displayLoseGame;

        private int _allCountMoneyPlayer;
        private int _allCountHealthPlayer;
        private int _allCountArmorPlayer;
        private int _allCountPowerPlayer;

        private DataPlayer _money;
        private DataPlayer _heath;
        private DataPlayer _power;
        private DataPlayer _armor;

        private Enemy _enemy;


        private void Start()
        {
            _enemy = new Enemy("Enemy Flappy");

            _money = CreateDataPlayer(DataType.Money);
            _heath = CreateDataPlayer(DataType.Health);
            _power = CreateDataPlayer(DataType.Power);
            _armor = CreateDataPlayer(DataType.Armor);

            Subscribe();
        }

        private void OnDestroy()
        {
            DisposeDataPlayer(ref _money);
            DisposeDataPlayer(ref _heath);
            DisposeDataPlayer(ref _power);
            DisposeDataPlayer(ref _armor);

            Unsubscribe();
        }


        private DataPlayer CreateDataPlayer(DataType dataType)
        {
            DataPlayer dataPlayer = new DataPlayer(dataType);
            dataPlayer.Attach(_enemy);

            return dataPlayer;
        }

        private void DisposeDataPlayer(ref DataPlayer dataPlayer)
        {
            dataPlayer.Detach(_enemy);
            dataPlayer = null;
        }


        private void Subscribe()
        {
            _addMoneyButton.onClick.AddListener(IncreaseMoney);
            _minusMoneyButton.onClick.AddListener(DecreaseMoney);

            _addHealthButton.onClick.AddListener(IncreaseHealth);
            _minusHealthButton.onClick.AddListener(DecreaseHealth);

            _addArmorButton.onClick.AddListener(IncreaseArmor);
            _minusArmorButton.onClick.AddListener(DecreaseArmor);
            
            _addPowerButton.onClick.AddListener(IncreasePower);
            _minusPowerButton.onClick.AddListener(DecreasePower);

            _fightButton.onClick.AddListener(Fight);
        }

        private void Unsubscribe()
        {
            _addMoneyButton.onClick.RemoveAllListeners();
            _minusMoneyButton.onClick.RemoveAllListeners();

            _addHealthButton.onClick.RemoveAllListeners();
            _minusHealthButton.onClick.RemoveAllListeners();

            _addArmorButton.onClick.RemoveAllListeners();
            _minusArmorButton.onClick.RemoveAllListeners();

            _addPowerButton.onClick.RemoveAllListeners();
            _minusPowerButton.onClick.RemoveAllListeners();

            _fightButton.onClick.RemoveAllListeners();
        }


        private void IncreaseMoney() => IncreaseValue(ref _allCountMoneyPlayer, DataType.Money);
        private void DecreaseMoney() => DecreaseValue(ref _allCountMoneyPlayer, DataType.Money);

        private void IncreaseHealth() => IncreaseValue(ref _allCountHealthPlayer, DataType.Health);
        private void DecreaseHealth() => DecreaseValue(ref _allCountHealthPlayer, DataType.Health);
        private void IncreaseArmor() => IncreaseValue(ref _allCountArmorPlayer, DataType.Armor);
        private void DecreaseArmor() => DecreaseValue(ref _allCountArmorPlayer, DataType.Armor);
        private void IncreasePower() => IncreaseValue(ref _allCountPowerPlayer, DataType.Power);
        private void DecreasePower() => DecreaseValue(ref _allCountPowerPlayer, DataType.Power);

        private void IncreaseValue(ref int value, DataType dataType) => AddToValue(ref value, 1, dataType);
        private void DecreaseValue(ref int value, DataType dataType) => AddToValue(ref value, -1, dataType);

        private void AddToValue(ref int value, int addition, DataType dataType)
        {
            value += addition;
            ChangeDataWindow(value, dataType);
        }


        private void ChangeDataWindow(int countChangeData, DataType dataType)
        {
            DataPlayer dataPlayer = GetDataPlayer(dataType);
            TMP_Text textComponent = GetTextComponent(dataType);
            string text = $"Player {dataType:F} {countChangeData}";

            dataPlayer.Value = countChangeData;
            textComponent.text = text;

            int enemyPower = _enemy.CalcPower();
            _countPowerEnemyText.text = $"Enemy Power {enemyPower}";
        }

        private TMP_Text GetTextComponent(DataType dataType) =>
            dataType switch
            {
                DataType.Money => _countMoneyText,
                DataType.Health => _countHealthText,
                DataType.Power => _countPowerText,
                DataType.Armor => _countArmorText,
                _ => throw new ArgumentException($"Wrong {nameof(DataType)}")
            };

        private DataPlayer GetDataPlayer(DataType dataType) =>
            dataType switch
            {
                DataType.Money => _money,
                DataType.Health => _heath,
                DataType.Power => _power,
                DataType.Armor => _armor,
                _ => throw new ArgumentException($"Wrong {nameof(DataType)}")
            };


        private void Fight()
        {
            int enemyPower = _enemy.CalcPower();
            bool isVictory = _allCountPowerPlayer >= enemyPower;

            if(isVictory)
            {
                _displayWonGame.gameObject.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else
            {
                _displayLoseGame.gameObject.SetActive(true);
                Time.timeScale = 0.0f;
            }
          
        }

       
    }
}
