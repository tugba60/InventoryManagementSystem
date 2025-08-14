# ✨ InventoryManagementSystem
An inventory management system designed for maintenance and repair units. (Bakım onarım birimleri için geliştirilmiş envanter yönetim sistemi.)

## [EN] ✨Maintenance and Inventory Management System✨ 

### Overview
An inventory management system designed for maintenance and repair units, especially in the defense industry. The system supports **user and admin roles**, allowing tracking of products, recording maintenance activities, and keeping detailed logs of actions.

### Features
  - User & Admin role-based access
  - Account Creation & Login
  - Product tracking with detailed attributes  
  - Maintenance record management  
  - Log system for every action  
  - SQL Server database integration  
  - Secure password storage with hashing  
  - Multi-form WinForms interface
    
### Database Structure
**Tables:**  
  - `Users` → User information and hashed passwords  
  - `Products` → Inventory items (name, code, category, brand, quantity, etc.)  
  - `Maintenance` → Maintenance records for products
  - `Transactions` → Transactions stores all inventory-related activities
  - `Logs` → User action tracking
    
###  🛠 Technologies Used
  - **C# WinForms**   
  - **.NET Framework**  
  - **SQL Server** 
  - **ADO.NET** for database operations  
  - **PBKDF2** for password hashing   

### 🚀 Installation
1. Clone the repository.
   ```bash
   git clone https://github.com/tugba60/InventoryManagementSystem.git
2. Import the SQL script into SQL Server.
3. Update the ConnectionString in the config file.
4. Run the project in Visual Studio.

   
*******************************************************************************


## [TR] ✨Bakım Onarım ve Envanter Yönetim Sistemi✨ 

### Genel Bakış
Özellikle savunma sanayii bakım ve onarım birimleri için geliştirilmiş envanter yönetim sistemidir. **Kullanıcı rollerine (user ve admin)** göre giriş imkanı tanıyarak ürün takibi, bakım faaliyetleri kaydı ve tüm işlemlerin log kayıtlarının tutulmasını sağlar.

### Özellikler
  - Kullanıcı ve admin rol tabanlı erişim
  - Hesap oluşturma ve hesap açma
  - Detaylı ürün takibi  
  - Bakım kayıt yönetimi  
  - Her işlem için log sistemi  
  - SQL Server veri tabanı entegrasyonu  
  - Hash’lenmiş şifre ile güvenli giriş
  - Çok formlu WinForms arayüzü
    
### Veri Tabanı Yapısı
**Tablolar:**
  - `Users` → Kullanıcı bilgileri ve Hash'lenmiş şifreleri
  - `Products` → Envanterdeki ürünler (isim, kod, kategori, marka, adet, vb.)  
  - `Maintenance` → Ürünlerin bakım ve onarım kayıtları
  - `Transactions` → Enavterdeki ürünler için işlem kaydı
  - `Logs` → Kullanıcıların yaptıkları işlemlerin kayıtları
    
###  🛠 Kullanılan Teknolojiler
  - **C# WinForms**   
  - **.NET Framework**  
  - **SQL Server** 
  - **ADO.NET** fveritabanı işlemleri için  
  - **PBKDF2** şifre hash'lemek için

### 🚀 Kurulum
1. Depoyu Klonla.
   ```bash
   git clone https://github.com/tugba60/InventoryManagementSystem.git
2. SQL script dosyasını SQL Server’a aktar.
3. ConnectionString ayarını config dosyasında güncelle.
4. Projeyi Visual Studio’da çalıştır.
