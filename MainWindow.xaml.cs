using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace YourProjectName
{
  public partial class MainWindow : Window
  {
    private string correctAnswer;
    private bool isCpuSocketTest, isRamTest, isPcieTest, isBiosTest, isQuartzTest, isBiosBatteryTest, is12VTest, is5VTest, is3_3VTest, isUsbTest, isResistanceTest;
    private string explanation;

    public MainWindow()
    {
      InitializeComponent();
    }

    private void TestMethodToggleButton_Click(object sender, RoutedEventArgs e)
    {
      ToggleButton clickedButton = sender as ToggleButton;
      foreach (var button in new[] { ResistanceButton, VoltageButton, CurrentButton, OscilloscopeButton, RamTesterButton, PcieTesterButton, CpuSocketTesterButton })
      {
        if (button != clickedButton)
          button.IsChecked = false;
      }
      clickedButton.IsChecked = true;
      ResetFlagsAndUI();
    }

    private void ResetFlagsAndUI()
    {
      isCpuSocketTest = isRamTest = isPcieTest = isBiosTest = isQuartzTest = isBiosBatteryTest = is12VTest = is5VTest = is3_3VTest = isUsbTest = isResistanceTest = false;
      HideQuestionAndAnswers();
      OscilloscopeImage.Visibility = Visibility.Collapsed;
    }

    private void Voltage3_3V_Click(object sender, RoutedEventArgs e)
    {
      if (VoltageButton.IsChecked == true)
        TestVoltage(3.3, 3.0, 3.6, "линии 3.3V", "+3,3 ± 0,3V");
      else if (ResistanceButton.IsChecked == true)
        TestResistance(100, 10000, "линии 3.3V", "от 100 Ом до 10 кОм");
      else
        OutputTextBlock.Text = "Попробуйте использовать другой инструмент.";
    }

    private void Voltage5V_Click(object sender, RoutedEventArgs e)
    {
      if (VoltageButton.IsChecked == true)
        TestVoltage(5, 4.5, 5.5, "линии 5V", "+5 ± 0,5V");
      else if (ResistanceButton.IsChecked == true)
        TestResistance(100, 10000, "линии 5V", "от 100 Ом до 10 кОм");
      else
        OutputTextBlock.Text = "Попробуйте использовать другой инструмент.";
    }

    private void Voltage12V_Click(object sender, RoutedEventArgs e)
    {
      if (VoltageButton.IsChecked == true)
        TestVoltage(12, 11.8, 13.2, "линии 12V", "+12V ± 1,2V");
      else if (ResistanceButton.IsChecked == true)
        TestResistance(1000, 20000, "линии 12V", "от 1 кОм до 20 кОм");
      else
        OutputTextBlock.Text = "Попробуйте использовать другой инструмент.";
    }

    private void TestVoltage(double nominalVoltage, double minVoltage, double maxVoltage, string lineName, string normalRange)
    {
      Random rand = new Random();
      double voltage = (rand.Next(2) == 0) ? (nominalVoltage - 1) + rand.NextDouble() * 2 : minVoltage + rand.NextDouble() * (maxVoltage - minVoltage);
      OutputTextBlock.Text = $"Напряжение {lineName}: {voltage:F1} В";
      correctAnswer = (voltage >= minVoltage && voltage <= maxVoltage) ? "Да" : "Нет";
      explanation = $"Нормальные значения для {lineName} должны быть в диапазоне {normalRange}.";
      ShowQuestionAndAnswers();
    }

    private void TestResistance(double minResistance, double maxResistance, string lineName, string normalRange)
    {
      Random rand = new Random();
      double resistance = (rand.Next(2) == 0) ? rand.NextDouble() * minResistance : minResistance + rand.NextDouble() * (maxResistance - minResistance);
      OutputTextBlock.Text = $"Сопротивление {lineName}: {resistance:F1} Ом";
      correctAnswer = (resistance >= minResistance && resistance <= maxResistance) ? "Да" : "Нет";
      explanation = $"Нормальные значения сопротивлений для {lineName} должны быть в диапазоне {normalRange}.";
      ShowQuestionAndAnswers();
    }

    private void SocketButton_Click(object sender, RoutedEventArgs e)
    {
      if (CpuSocketTesterButton.IsChecked == true)
        TestRandom("сокета ЦПУ", new[] { "Все индикаторы горят красным", "Горит только половина индикаторов", "Индикаторы не горят" }, "Да", "Нет", "при исправном сокете процессора все индикаторы должны гореть красным.");
      else
        OutputTextBlock.Text = "Попробуйте использовать другой инструмент.";
    }

    private void RamButton_Click(object sender, RoutedEventArgs e)
    {
      if (RamTesterButton.IsChecked == true)
        TestRandom("слота ОЗУ", new[] { "Все индикаторы горят красным", "Горит только половина индикаторов", "Индикаторы не горят" }, "Да", "Нет", "при исправном слоте ОЗУ все индикаторы должны гореть красным.");
      else
        OutputTextBlock.Text = "Попробуйте использовать другой инструмент.";
    }

    private void PcieButton_Click(object sender, RoutedEventArgs e)
    {
      if (PcieTesterButton.IsChecked == true)
        TestRandom("слота PCI-E", new[] { "Есть сигнал", "Нет сигнала" }, "Да", "Нет", "при исправно работающей шине должен быть сигнал.");
      else
        OutputTextBlock.Text = "Попробуйте использовать другой инструмент.";
    }

    private void TestRandom(string element, string[] messages, string correctMsg, string incorrectMsg, string additionalExplanation)
    {
      Random rand = new Random();
      int index = rand.Next(messages.Length);
      OutputTextBlock.Text = messages[index];
      correctAnswer = (index == 0) ? correctMsg : incorrectMsg;
      explanation = $"Так как {additionalExplanation}";
      ShowQuestionAndAnswers();
    }

    private void BiosButton_Click(object sender, RoutedEventArgs e)
    {
      if (OscilloscopeButton.IsChecked == true)
        TestOscilloscope("BIOS", "Так как мы не видим осциллограммы на экране.", "Так как есть осциллограмма.");
      else
        OutputTextBlock.Text = "Попробуйте использовать другой инструмент.";
    }


    private void TestOscilloscope(string element, string incorrectExplanation, string correctExplanation)
    {
      string[] images = { "pack://application:,,,/faultyoscilloscope.jpg", "pack://application:,,,/workingoscilloscope.jpg" };
      Random rand = new Random();
      int index = rand.Next(images.Length);
      string selectedImage = images[index];

      BitmapImage bitmap = new BitmapImage(new Uri(selectedImage));
      OscilloscopeImage.Source = bitmap;
      OscilloscopeImage.Visibility = Visibility.Visible;

      correctAnswer = (index == 0) ? "Нет" : "Да";
      explanation = (index == 0) ? incorrectExplanation : correctExplanation;

      ShowQuestionAndAnswers();
    }

    private void BiosBatteryButton_Click(object sender, RoutedEventArgs e)
    {
      if (VoltageButton.IsChecked == true)
        TestVoltage(3.0, 2.7, 3.3, "батарейки BIOS", "+3 ± 0,3 вольт");
      else
        OutputTextBlock.Text = "Попробуйте использовать другой инструмент.";
    }

    private void UsbButton_Click(object sender, RoutedEventArgs e)
    {
      if (VoltageButton.IsChecked == true)
        TestVoltage(5.0, 4.75, 5.25, "линии USB", "5V ± 0,25V");
      else
        OutputTextBlock.Text = "Попробуйте использовать другой инструмент.";
    }

    private void AnswerButton_Click(object sender, RoutedEventArgs e)
    {
      string userAnswer = (sender as Button).Content.ToString();
      string resultMessage = (userAnswer == correctAnswer) ? "Верно." : "Вы дали неправильный ответ.";
      resultMessage += $" {explanation}";
      OutputTextBlock.Text = resultMessage;
      HideQuestionAndAnswers();
    }

    private void ShowQuestionAndAnswers()
    {
      QuestionTextBlock.Text = "Исправен ли этот элемент?";
      YesButton.Visibility = Visibility.Visible;
      NoButton.Visibility = Visibility.Visible;
    }

    private void HideQuestionAndAnswers()
    {
      QuestionTextBlock.Text = string.Empty;
      YesButton.Visibility = Visibility.Collapsed;
      NoButton.Visibility = Visibility.Collapsed;
      OscilloscopeImage.Visibility = Visibility.Collapsed;
    }
  }
}