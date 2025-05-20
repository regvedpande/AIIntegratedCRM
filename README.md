
# 🚀 AIIntegratedCRM

AIIntegratedCRM is a modern, AI-powered CRM system built using **ASP.NET Core MVC 8**, **Razor Pages**, and **MSSQL**, integrated with **Google Gemini API** to deliver intelligent summaries, communication assistance, and futuristic user experience. Built for speed, simplicity, and impact.

---

## 🌟 Features

- ✅ **Full CRUD**: Manage Customers effortlessly with clean UI and server-side validations.
- 🧠 **AI Integration (Gemini)**: Generate smart, human-like summaries for customer profiles.
- 🔐 **Authentication**: Secure login/logout with Identity.
- 💌 **Email Messaging (SMTP-ready)**: Send emails directly from CRM (future scope).
- 📊 **Clean Dashboard**: Real-time data views, fast navigation, responsive layout.
- 🛠️ **Razor Pages + MVC Hybrid**: Blends best of both worlds for rapid development.
- ⚡ **.NET 8 Optimizations**: Leverages latest features in ASP.NET Core 8 for performance.

---

## 🏗️ Tech Stack

| Layer          | Technology                              |
|----------------|------------------------------------------|
| Frontend       | Razor Pages, Bootstrap 5                 |
| Backend        | ASP.NET Core MVC 8                       |
| AI Integration | Google Gemini 2.0 API (Text Generation)  |
| Database       | Microsoft SQL Server (EF Core)           |
| ORM            | Entity Framework Core                    |
| Auth           | ASP.NET Identity                         |
| IDE            | Visual Studio 2022                       |

---

## 📸 UI Snapshot

![Dashboard](screenshots/dashboard.png)  
*A clean and elegant UI to manage customers and AI interactions.*

---

## 📦 Installation

1. **Clone the Repo**

   git clone https://github.com/yourusername/AIIntegratedCRM.git
   cd AIIntegratedCRM

2. **Configure Connection Strings**

   Update your `appsettings.json` with your SQL Server credentials:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=AIIntegratedCRM;Trusted_Connection=True;"
   }
   ```

3. **Add Gemini API Key**

   Add this to `appsettings.json`:

   ```json
   "Gemini": {
     "ApiKey": "YOUR_GOOGLE_GEMINI_API_KEY"
   }
   ```

4. **Run the Project**

   ```bash
   dotnet build
   dotnet ef database update
   dotnet run
   ```

---

## 🧠 How the AI Works

Every time a customer is created or viewed, a background service sends the customer data to Google Gemini’s `generateContent` endpoint. The AI returns a clean, human-like message tailored to that customer — visible in the CRM UI.

```csharp
// Sample AI prompt
"Generate a friendly message for customer Regved Pande from Cylsys Software."
```

---

## 🚧 Roadmap

* [ ] AI Chat interface for customer support agents
* [ ] Role-based user management (Admin, Agent, Viewer)
* [ ] Email automation with scheduling
* [ ] Activity logs and customer timeline
* [ ] Export to Excel/CSV
* [ ] Mobile-friendly UI redesign (Blazor or MAUI in future?)

---

## 🤖 System Requirements

* .NET 8 SDK
* Visual Studio 2022+
* SQL Server 2019+
* Google Gemini API Key
* Windows 10/11 or macOS (via VS Code + CLI)

---

## 🤝 Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

---

## 📜 License

MIT License. See [`LICENSE`](LICENSE) for more information.

---

## 🙇‍♂️ Author

**Regved Pande**
💼 Full Stack .NET Developer
📫 [regregd@outlook.com](mailto:regregd@outlook.com)
🌍 India 🇮🇳

---

## 🧬 Inspired by the Vision of a Futuristic, AI-Augmented World

> "One man. One dream. A CRM powered by AI — built to go beyond just managing customers... it understands them."

---

## 🌌 Bonus: Future Vision

Imagine a CRM that doesn’t just *store* data but *thinks*.
AIIntegratedCRM is the first step toward that dream.

