using System;
using System.Numerics;
using System.Text;

public struct KeysParams {
    public BigInteger n;
    public BigInteger e;
    public BigInteger d;
}

public class Core {
    private const string ab = "0123456789АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя ";

    /// <summary>
    /// Генерирует пару ключей (публичный и приватный).
    /// </summary>
    /// <returns>Структура KeysParams, содержащая n, e и d.</returns>
    public static KeysParams generate_keys() {
        Random rnd = new Random();
        // Дают произведения не меньше 77 и не больше 75^2 (т.к. каждая буква входа кодируется в две на выходе)
        int[] primes = { 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73 };

        BigInteger p = 0, q = 0, n = 0;

        while (n < 77 || n > 5625 || p == q) {
            p = primes[rnd.Next(primes.Length)];
            q = primes[rnd.Next(primes.Length)];
            n = p * q;
        }

        BigInteger p1 = p - 1;
        BigInteger q1 = q - 1;
        BigInteger l = (p1 * q1) / BigInteger.GreatestCommonDivisor(p1, q1);

        BigInteger e = 3;
        while (e < l) {
            if ( new Func<BigInteger, bool>((number) => { // Видать это единственный способ в C# создать лямбду и сразу вызвать
                if (number < 2)
                    return false;
                for (int i = 2; i * i <= number; i++) {
                    if (number % i == 0)
                        return false;
                }
                return true;
            })(e) && l % e != 0) {
                break;
            }
            e++;
        }

        BigInteger d = 1;
        while ((d * e) % l != 1) {
            d++;
        }

        return new KeysParams { n = n, e = e, d = d };
    }

    /// <summary>
    /// Зашифровывает исходный текст посимвольно. Каждый символ отображается ровно в два символа.
    /// </summary>
    /// <param name="text">Исходный нешифрованный текст.</param>
    /// <param name="n">Модуль (часть публичного ключа).</param>
    /// <param name="e">Экспонента (часть публичного ключа).</param>
    /// <returns>Зашифрованная строка.</returns>
    /// <exception cref="Exception">Выбрасывается при некорректном вводе.</exception>
    public static string encrypt(string text, BigInteger n, BigInteger e) {
        if (text == null || n == null || e == null)
            throw new Exception("Параметры не могут быть null");
        if (n < 77)
            throw new Exception("Модуль n должен быть не меньше 77");

        StringBuilder sb = new StringBuilder(text.Length * 2); // Технологично

        foreach (char c in text) {
            int m = ab.IndexOf(c);
            if (m == -1)
                throw new Exception($"Символ '{c}' не входит в алфавит.");

            BigInteger cVal = BigInteger.ModPow(m, e, n);

            int high = (int)(cVal / 77);
            int low  = (int)(cVal % 77);

            sb.Append(ab[high]);
            sb.Append(ab[low]);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Расшифровывает текст, преобразуя каждые два зашифрованных символа обратно в один исходный.
    /// </summary>
    /// <param name="text">Зашифрованный текст.</param>
    /// <param name="n">Модуль (часть приватного ключа).</param>
    /// <param name="d">Экспонента (часть приватного ключа).</param>
    /// <returns>Расшифрованная строка.</returns>
    /// <exception cref="Exception">Выбрасывается при некорректном вводе.</exception>
    public static string decrypt(string text, BigInteger n, BigInteger d) {
        if (text == null || n == null || d == null)
            throw new Exception("Параметры не могут быть null");
        if (n < 77)
            throw new Exception("Модуль n должен быть не меньше 77");
        if (text.Length % 2 != 0)
            throw new Exception("Длина зашифрованного текста должна быть чётной.");

        StringBuilder sb = new StringBuilder(text.Length / 2);

        for (int i = 0; i < text.Length; i += 2) {
            int high = ab.IndexOf(text[i]);
            int low = ab.IndexOf(text[i + 1]);

            if (high == -1 || low == -1)
                throw new Exception("Некорректный текст.");

            BigInteger cVal = high * 77 + low;

            BigInteger mVal = BigInteger.ModPow /* технологично */ (cVal, d, n);

            if (mVal >= 77)
                throw new Exception("Некорректный текст.");

            sb.Append(ab[(int)mVal]);
        }

        return sb.ToString();
    }
}
