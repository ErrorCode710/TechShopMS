---

### ✅ **🗂️ PROJECT STRUCTURE & SETUP**

* [x] Setup Avalonia UI project
* [x] Install CommunityToolkit.MVVM
* [x] Setup folder structure (`Views`, `ViewModels`, `Models`, `Services`, `Data`)
* [x] Setup App.xaml with:

  * [x] Color Palette
  * [x] Fonts
  * [x] Global Styles
 

---

### ✅ **🏠 UI NAVIGATION**

* [x] SplitView layout for sidebar tabs
* [x] Home tab with system overview
* [x] Sidebar items for each module:

  * [x] Home
  * [x] Inventory
  * [x] Sales
  * [x] Customers
  * [x] Reports
  * [x] Users / Settings

---

### ✅ **📦 PRODUCT / INVENTORY MANAGEMENT**

* [x] `Product` model:

  * [ ] ID (auto-increment in DB)
  * [ ] Name, Category, Brand
  * [ ] Price, Quantity
  * [ ] SKU or Part Number
  * [ ] Description / Specs
  * [ ] Product Image (optional for now)
* [ ] CRUD UI for Products
* [ ] DataGrid for listing products
* [ ] Filtering & search (by category, brand, etc.)

---

### ✅ **🧾 SALES MANAGEMENT**

* [ ] Sales model: Invoice ID, Date, Customer, Items, Total, Discounts, Tax
* [ ] Cart-like UI: Add multiple products per sale
* [ ] Payment method (cash, GCash, etc.)
* [ ] Save and print receipt (optional)
* [ ] Sales list with filtering by date

---

### ✅ **👥 CUSTOMER MANAGEMENT**

* [ ] Customer model: ID, Name, Phone, Email, Address
* [ ] Customer history (list of purchases)
* [ ] CRUD for customers

---

### ✅ **📊 REPORTING**

* [ ] Inventory Report (low stock, category count)
* [ ] Sales Report (daily, weekly, monthly)
* [ ] Customer Report (top customers, total purchases)
* [ ] Export reports to PDF/CSV (bonus)

---

### ✅ **🔐 USER MANAGEMENT**

* [x] User model: ID, Name, Username, Password (hashed), Role
* [x] Login window
* [ ] Role-based access control (Admin vs Staff)
* [ ] Full CRUD for usersmanagement (Admin only)
* [ ] Full CRUD for Inventory (Admin & Manager)
* [ ] View/Add Inventory (Staff)

---

### ✅ **🧱 DATABASE INTEGRATION**

* [ ] Use SQLite (for simplicity)
* [ ] Create tables for Products, Sales, Customers, Users
* [ ] Basic SQL (CREATE, INSERT, SELECT, UPDATE, DELETE)
* [ ] Repository or Service pattern for data access

---

### ✅ **🧪 OTHER NICE-TO-HAVES**

* [ ] Dark/light mode toggle
* [ ] Settings tab (App settings like themes, currency)
* [ ] App version info / About section
* [ ] Backup & restore DB feature (manual or auto)


