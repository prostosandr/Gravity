# 🌌 Gravity: Puzzle Platformer

![Gameplay Showcase]([https://drive.google.com/uc?export=view&id=16YKC-OFHCLfgSbXpWjiEQchynn5krm7M](https://drive.google.com/file/d/16YKC-OFHCLfgSbXpWjiEQchynn5krm7M/view?usp=drive_link))

**Gravity** — это хардкорный 2D-платформер, построенный на пространственных головоломках и управлении физикой. Главная особенность игры — манипуляция вектором гравитации, позволяющая перемещаться по стенам, потолкам и использовать инерцию для преодоления препятствий.

---

### 🎮 Ключевые механики

**1. Инверсия гравитации**
Способность мгновенно менять вектор падения. Открывает простор для нестандартного платформинга, обхода смертельных ловушек и перемещения по потолку.

![Gravity Inversion](https://drive.google.com/uc?export=view&id=Exgr6PvWHt_UI8rpHVObZt_zeeBS5EoX)

**2. Гравитационные колодцы (Gravity Wells)**
Локальные зоны с измененным притяжением. Игрок может использовать эти поля, чтобы менять плоскость движения на лету или применять физическую инерцию для катапультирования на дальние расстояния.

![Gravity Wells Interaction](https://drive.google.com/uc?export=view&id=XrDWQ5ttc7-XNAUdv9U6ssDVU1aBn7FZ)

**3. Хардкорный дизайн уровней**
Локации требуют высокой точности, микроконтроля, быстрого чтения таймингов и понимания физики движения. Цена ошибки высока, но механики работают предсказуемо и отзывчиво.

![Hardcore Platforming](https://drive.google.com/uc?export=view&id=19X2hP9KhASpPMBaze-zo1M5uPRi0l2la)

<details>
<summary><b>🎬 Развернуть галерею геймплея (Еще 4 гифки)</b></summary>
<br>
  
![Gameplay 4](https://drive.google.com/uc?export=view&id=NKn2QN0kZzFDYfaGio13KSZGrkCJdF3W)
![Gameplay 5](https://drive.google.com/uc?export=view&id=192Nj6XE2l0nsgX7xxWkC4EO_AKa3YI2_)
![Gameplay 6](https://drive.google.com/uc?export=view&id=dXLr0AeJFRwFImxR9ByDNZCT8Ur4-aMx)
![Gameplay 7](https://drive.google.com/uc?export=view&id=MI5QW1nVc7HEy9xNRc1wZvRfFtNDZU07)

</details>

---

### 🛠 Техническая реализация (Unity / C#)
Архитектура проекта выстроена с прицелом на модульность и легкость масштабирования, опираясь на принципы **SOLID** и компонентный подход Unity:
* **Изолированная физика:** Вся логика манипуляции гравитацией разбита на независимые компоненты. Классы `GravityWellInteraction` и `GravityInverter` инкапсулируют логику локальных полей и инверсии, не перегружая основной контроллер игрока.
* **Разделение ответственности:** Системы считывания ввода, обработки перемещения и реакции на окружение (шипы, платформы) изолированы друг от друга. Это позволяет легко конструировать новые типы гравитационных аномалий и расширять функционал без изменения ядра физики.

---

### 🕹️ Управление / Controls

Игра полностью поддерживает как классическую раскладку мышь + клавиатура, так и управление с геймпада:

| Действие / Action | ⌨️ Клавиатура и мышь | 🎮 Геймпад |
| :--- | :--- | :--- |
| **Перемещение** | `A` / `D` | Left Stick (Влево /
