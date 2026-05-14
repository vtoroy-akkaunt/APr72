using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace APr72 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Random _random = new Random();

        /// <summary>
        /// Конструктор главного окна.
        /// </summary>
        public MainWindow() {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события ввода текста. Блокирует ввод любых символов, кроме цифр.
        /// </summary>
        private void validate(object sender, TextCompositionEventArgs e) {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        /// <summary>
        /// Обработчик кнопки "Сгенерировать ключи".
        /// </summary>
        private void btn_generate_Click(object sender, RoutedEventArgs eArgs) {
            var keys = Core.generate_keys();
            n.Text = keys.n.ToString();
            e.Text = keys.e.ToString();
            d.Text = keys.d.ToString();
        }

        /// <summary>
        /// Обработчик кнопки "Зашифровать".
        /// </summary>
        private void btn_encrypt_Click(object sender, RoutedEventArgs eArgs) {
            if (string.IsNullOrWhiteSpace(the_text.Text)) {
                MessageBox.Show("Поле ввода пустое.");
                return;
            }

            if (
                !BigInteger.TryParse(e.Text, out BigInteger e_) ||
                !BigInteger.TryParse(n.Text, out BigInteger n_) ||
                n_ <= 1 || e_ <= 1
            ) {
                MessageBox.Show("Невалидные параметры.");
                return;
            }
            try {
                the_text.Text = Core.encrypt(the_text.Text, n_, e_);
            } catch {
                MessageBox.Show("Некорректный ввод.");
            }
        }

        /// <summary>
        /// Обработчик кнопки "Расшифровать". Расшифровывает текст (числа через пробел) обратно в символы (TC_03).
        /// </summary>
        private void BtnDecrypt_Click(object sender, RoutedEventArgs eArgs) {
            if (string.IsNullOrWhiteSpace(the_text.Text)) {
                MessageBox.Show("Поле ввода пустое.");
                return;
            }

            if (
                !BigInteger.TryParse(d.Text, out BigInteger d_) ||
                !BigInteger.TryParse(n.Text, out BigInteger n_) ||
                n_ <= 1 || d_ <= 1
            ) {
                MessageBox.Show("Невалидные параметры.");
                return;
            }
            try {
                the_text.Text = Core.decrypt(the_text.Text, n_, d_);
            } catch {
                MessageBox.Show("Некорректный ввод.");
            }
        }
    }
}
