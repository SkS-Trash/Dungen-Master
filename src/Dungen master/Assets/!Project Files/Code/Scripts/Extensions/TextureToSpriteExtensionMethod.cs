using UnityEngine;

/// <summary>
/// Метод расширения для преобразования текстуры2D в спрайт.
/// </summary>
public static class TextureToSpriteExtensionMethod
{
    /// <summary>
    /// Преобразование текстуры2D в спрайт.
    /// </summary>
    /// <param name="texture">Текстура2D.</param>
    /// <returns>Спрайт.</returns>
    public static Sprite ToSprite(this Texture2D texture)
    {
        var rect = new Rect(0, 0, texture.width, texture.height);
        return Sprite.Create(texture, rect, Vector2.zero);
    }
}