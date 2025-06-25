# Mini Account Management System

A simple accounting system built using ASP.NET Core Razor Pages with role-based access control. The application provides functionality to manage:

- Chart of accounts with parent-child hierarchy
- User roles (Admin, Accountant, Viewer)
- Role-based access to modules (view/edit)
- Voucher entries (add, edit, delete)
- Account tree rendering

---

## Features

- Role-based permissions (Admin, Accountant, Viewer)
- Manage users and assign roles
- Display and manage hierarchical account trees
- Voucher management (Create, Edit, Delete)
- Module-level access control via stored procedures

---

## Technologies Used

- ASP.NET Core Razor Pages
- SQL Server
- Identity (User & Role Management)

---

## Project overview with screenshots
1.ASP.NET Core Identity is used to handle authentication and authorization on the default Home page.
![image alt](https://github.com/nusrat463/MiniAccountManagementSystem/blob/main/2.PNG?raw=true)
2.User Management page includes update and delete functionality, with users fetched from the registration.
![image alt](https://github.com/nusrat463/MiniAccountManagementSystem/blob/b116597fd286c39c230af5a5bf048d80ca493605/3.PNG)
3.Role Management with add role and delete role functionalities.
![image alt](https://github.com/nusrat463/MiniAccountManagementSystem/blob/b116597fd286c39c230af5a5bf048d80ca493605/4.PNG)
4.Module assign to roles using SP.
![image alt](https://github.com/nusrat463/MiniAccountManagementSystem/blob/b116597fd286c39c230af5a5bf048d80ca493605/5.PNG)
5.Admin can only view Charts of Accounts tree, can not add account.
![image alt](https://github.com/nusrat463/MiniAccountManagementSystem/blob/b116597fd286c39c230af5a5bf048d80ca493605/6.PNG)
6.Accountant have been assigned add permission of Accounts.
![image alt](https://github.com/nusrat463/MiniAccountManagementSystem/blob/b116597fd286c39c230af5a5bf048d80ca493605/7.PNG)



