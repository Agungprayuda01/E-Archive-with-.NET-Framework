Ini adalah project file website yang dibuat menggunakan .NET Framework dengan nama E-Archive

E-archive sendiri berfungsi untuk menyimpan file atau dokemen ke website
E-Archive dibuat untuk menjadi backup dokemen penting kalian ke digital

cara pakai project ini pertama kalian harus install VS 2022 dan pastikan menginstall .NET Framework
setelah itu bisa kalian clone atau kalian download project ini
pertama setting server di appsetting.json di bagian "ConnectionString" sesuaikan dengan server lokal kalian
setelah itu buka tool lalu pergi ke Nugget Package Manager lalu pergi ke Package Manager Console
lalu jalankan perintah "update-database"
jalankan dan buat akun halaman login
masukan email pass seterah kalian
matikan [Authorize(Roles = "Admin")] di controller data pegawai untuk memberikan role admin
tambahkan data dengan email yang kalian pakai daftar
klik edit dan ganti role ke admin "membuat akun yang kalian buat sebelumnya menjadi admin"
setelah itu kembalikan [Authorize(Roles = "Admin")] dan website siap di pakai
