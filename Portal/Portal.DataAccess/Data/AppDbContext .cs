using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Portal.Models;
using Portal.Models.Entitys;
using Portal.Models.Entitys.MasterData;
using Portal.Models.Entitys.NhanSu;

namespace Portal.DataAccess
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        #region Permission
        public DbSet<Account> Accounts { get; set; }
        public DbSet<RolesModel> RolesModels { get; set; }
        public DbSet<AccountInRoleModel> AccountInRoleModels { get; set; }
        public DbSet<MenuModel> MenuModels { get; set; }
        public DbSet<FunctionModel> FunctionModels { get; set; }
        public DbSet<PageFunctionModel> PageFunctionModels { get; set; }
        public DbSet<PagePermissionModel> PagePermissionModels { get; set; }
        #endregion
        public DbSet<HistoryModel> HistoryModels { get; set; }
        #region Masterdata
        public DbSet<Department> Departments { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<CoSo> CoSos { get; set; }
        public DbSet<NgachLuong> NgachLuongs { get; set; }
        public DbSet<BacLuong> BacLuongs { get; set; }
        public DbSet<TrinhDoTinHoc> TrinhDoTinHocs { get; set; }
        public DbSet<TrinhDoNgoaiNgu> TrinhDoNgoaiNgus { get; set; }
        public DbSet<NgonNgu> NgonNgus { get; set; }
        public DbSet<TrinhDoChinhTri> TrinhDoChinhTris { get; set; }
        public DbSet<TrinhDoHocVan> TrinhDoHocVans { get; set; }
        public DbSet<ChucVu> ChucVus { get; set; }
        public DbSet<DanToc> DanTocs { get; set; }
        public DbSet<TonGiao> TonGiaos { get; set; }
        public DbSet<KhuVuc> KhuVucs { get; set; }
        public DbSet<TinhThanhPho> TinhThanhPhos { get; set; }
        public DbSet<QuanHuyen> QuanHuyens { get; set; }
        public DbSet<PhuongXa> PhuongXas { get; set; }
        public DbSet<HocHam> HocHams { get; set; }
        public DbSet<TinhTrang> TinhTrangs { get; set; }
        public DbSet<HeDaoTao> HeDaoTaos { get; set; }
        public DbSet<PhanHe> PhanHes { get; set; }
        public DbSet<LoaiQuanHeGiaDinh> LoaiQuanHeGiaDinhs { get; set; }
      
        public DbSet<DanhHieuKhenThuong> DanhHieuKhenThuongs { get; set; }
        public DbSet<HinhThucKhenThuong> HinhThucKhenThuongs { get; set; }
        public DbSet<HinhThucKyLuat> HinhThucKyLuats { get; set; }
        public DbSet<HinhThucDaoTao> HinhThucDaoTaos { get; set; }
        public DbSet<LoaiBangCap> LoaiBangCaps { get; set; }
        public DbSet<ChuyenNganh> ChuyenNganhs { get; set; }
        #endregion
        #region nhanSu
        public DbSet<CanBo> CanBos { get; set; }
        public DbSet<QuanHeGiaDinh> QuanHeGiaDinhs { get; set; }
        public DbSet<QuaTrinhKhenThuong> QuaTrinhKhenThuongs { get; set; }
        public DbSet<QuaTrinhKyLuat> QuaTrinhKyLuats { get; set; }
        public DbSet<QuaTrinhDaoTao> QuaTrinhDaoTaos { get; set; }
        #endregion nhanSu

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Bỏ tiền tố AspNet của các bảng: mặc định các bảng trong IdentityDbContext có
            // tên với tiền tố AspNet như: AspNetUserRoles, AspNetUser ...
            // Đoạn mã sau chạy khi khởi tạo DbContext, tạo database sẽ loại bỏ tiền tố đó
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName!.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
            // Add department table 
            #region Master data
            builder.Entity<Department>(tbl =>
            {
                tbl.ToTable("Department", "tMasterData");

                tbl.HasKey(it => it.Id);
                tbl.Property(it => it.Code).IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("DepartmentCode");
                tbl.Property(it => it.Name).IsRequired()
                   .HasMaxLength(200)
                   .HasColumnName("DepartmentName");
                tbl.Property(it => it.Description).HasMaxLength(500);
                tbl.HasOne<CoSo>(it => it.CoSo)
                .WithMany(it => it.Departments)
                .HasForeignKey(it => it.IDCoSo)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Department_CoSo");
                
            });
            builder.Entity<Bank>(tbl =>
            {
                tbl.ToTable("Bank", "tMasterData");
            });
            builder.Entity<HinhThucDaoTao>(tbl =>
            {
                tbl.ToTable("HinhThucDaoTao", "tMasterData");
                tbl.HasKey(it => it.MaHinhThucDaoTao);
                tbl.Property(it => it.MaHinhThucDaoTao).IsRequired()
                  .HasMaxLength(50);
                tbl.Property(it => it.TenHinhThucDaoTao).IsRequired()
                   .HasMaxLength(200);
            });
            builder.Entity<LoaiBangCap>(tbl =>
            {
                tbl.ToTable("LoaiBangCap", "tMasterData");
                tbl.HasKey(it => it.MaLoaiBangCap);
                tbl.Property(it => it.MaLoaiBangCap).IsRequired()
                  .HasMaxLength(50);
                tbl.Property(it => it.TenLoaiBangCap).IsRequired()
                   .HasMaxLength(200);
            });
            builder.Entity<ChuyenNganh>(tbl =>
            {
                tbl.ToTable("ChuyenNganh", "tMasterData");
                tbl.HasKey(it => it.MaChuyenNganh);
                tbl.Property(it => it.MaChuyenNganh).IsRequired()
                  .HasMaxLength(50);
                tbl.Property(it => it.TenChuyenNganh).IsRequired()
                   .HasMaxLength(200);
            });
            builder.Entity<PhanHe>(tbl =>
            {
                tbl.ToTable("PhanHeModel", "tMasterData");
                tbl.HasKey(it => it.MaPhanHe);
            });
            builder.Entity<DanhHieuKhenThuong>(tbl =>
            {
                tbl.ToTable("DanhHieuKhenThuong", "tMasterData");
                tbl.HasKey(it => it.MaDanhHieuKhenThuong);
                tbl.Property(it => it.MaDanhHieuKhenThuong).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenDanhHieuKhenThuong).HasMaxLength(250).IsRequired(true);
            });
            builder.Entity<HinhThucKhenThuong>(tbl =>
            {
                tbl.ToTable("HinhThucKhenThuong", "tMasterData");
                tbl.HasKey(it => it.MaHinhThucKhenThuong);
                tbl.Property(it => it.MaHinhThucKhenThuong).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenHinhThucKhenThuong).HasMaxLength(250).IsRequired(true);
            });
            builder.Entity<LoaiQuanHeGiaDinh>(tbl =>
            {
                tbl.ToTable("LoaiQuanHeGiaDinh", "tMasterData");
                tbl.HasKey(it => it.IDLoaiQuanHeGiaDinh);
                tbl.Property(it => it.TenLoaiQuanHeGiaDinh).HasMaxLength(150).IsRequired(true);
            });
            builder.Entity<CoSo>(tbl =>
            {
                tbl.ToTable("CoSoModel", "tMasterData");
                tbl.HasKey(it => it.IdCoSo);
                tbl.Property(it => it.TenCoSo).HasMaxLength(200).IsRequired(true);
                tbl.Property(it => it.Description).HasMaxLength(500).IsRequired(false);
                tbl.Property(it => it.Actived).HasDefaultValue(true);
                tbl.Property(it => it.CreatedTime).HasDefaultValue(DateTime.Now);
            });
            builder.Entity<NgachLuong>(tbl =>
            {
                tbl.ToTable("NgachLuongModel", "tMasterData");
                tbl.HasKey(it => it.MaNgachLuong);
                tbl.Property(it => it.TenNgachLuong).HasMaxLength(200).IsRequired(true);
                tbl.Property(it => it.MaLoai).HasMaxLength(100).IsRequired(true);
                tbl.Property(it => it.NamTangLuong).IsRequired(true);
                tbl.Property(it => it.Description).HasMaxLength(500).IsRequired(false);
                tbl.Property(it => it.Actived).HasDefaultValue(true);
                tbl.Property(it => it.CreatedTime).HasDefaultValue(DateTime.Now);
            });
            builder.Entity<BacLuong>(tbl =>
            {
                tbl.ToTable("BacLuongModel", "tMasterData");
                tbl.HasKey(it => it.MaBacLuong);
                tbl.Property(it => it.MaNgachLuong).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenBacLuong).HasMaxLength(200).IsRequired(true);
                tbl.HasOne<NgachLuong>(it => it.NgachLuong)
                    .WithMany(it => it.BacLuongs)
                    .HasForeignKey(it => it.MaNgachLuong)
                    .HasConstraintName("FK_BacLuong_NgachLuong");
            });
            builder.Entity<NgonNgu>(tbl =>
            {
                tbl.ToTable("NgonNguModel", "tMasterData");
                tbl.HasKey(it => it.MaNgonNgu);
                tbl.Property(it => it.MaNgonNgu).HasMaxLength(50).IsRequired(true);
            });
            builder.Entity<TrinhDoNgoaiNgu>(tbl =>
            {
                tbl.ToTable("TrinhDoNgoaiNguModel", "tMasterData");
                tbl.HasKey(it => it.MaTrinhDoNgoaiNgu);
                tbl.Property(it => it.TenTrinhDoNgoaiNgu).HasMaxLength(200).IsRequired(true);
                tbl.Property(it => it.Description).HasMaxLength(500);
                tbl.HasOne<NgonNgu>(it => it.NgonNgu)
                .WithMany(it => it.TrinhDoNgoaiNgus)
                .HasForeignKey(it => it.MaNgonNgu)
                .HasConstraintName("FK_TrinhDoNgoaiNgu_TrinhDoNgoaiNgu");
            });
            builder.Entity<TrinhDoTinHoc>(tbl =>
            {
                tbl.ToTable("TrinhDoTinHocModel", "tMasterData");
                tbl.HasKey(it => it.MaTrinhDoTinHoc);
                tbl.Property(it => it.MaTrinhDoTinHoc).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenTrinhDoTinHoc).HasMaxLength(200).IsRequired(true);
                tbl.Property(it => it.Description).HasMaxLength(500);
            });
            builder.Entity<TrinhDoChinhTri>(tbl =>
            {
                tbl.ToTable("TrinhDoChinhTriModel", "tMasterData");
                tbl.HasKey(it => it.MaTrinhDoChinhTri);
                tbl.Property(it => it.MaTrinhDoChinhTri).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenTrinhDoChinhTri).HasMaxLength(200).IsRequired(true);
                tbl.Property(it => it.Description).HasMaxLength(500);
            });
            builder.Entity<TrinhDoHocVan>(tbl =>
            {
                tbl.ToTable("TrinhDoHocVanModel", "tMasterData");
                tbl.HasKey(it => it.MaTrinhDoHocVan);
                tbl.Property(it => it.MaTrinhDoHocVan).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenTrinhDoHocVan).HasMaxLength(200).IsRequired(true);
                tbl.Property(it => it.Description).HasMaxLength(500);
            });
            builder.Entity<ChucVu>(tbl =>
            {
                tbl.ToTable("ChucVuModel", "tMasterData");
                tbl.HasKey(it => it.MaChucVu);
                tbl.Property(it => it.TenChucVu).HasMaxLength(250).IsRequired(true);
                tbl.Property(it => it.HeSoChucVu).IsRequired(false);
                tbl.Property(it => it.Description).HasMaxLength(500);
            });
            builder.Entity<DanToc>(tbl =>
            {
                tbl.ToTable("DanTocModel", "tMasterData");
                tbl.HasKey(it => it.MaDanToc);
                tbl.Property(it => it.MaDanToc).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenDanToc).HasMaxLength(250).IsRequired(true);
                tbl.Property(it => it.Description).HasMaxLength(500).IsRequired(false);
            });
            builder.Entity<TonGiao>(tbl =>
            {
                tbl.ToTable("TonGiaoModel", "tMasterData");
                tbl.HasKey(it => it.MaTonGiao);
                tbl.Property(it => it.MaTonGiao).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenTonGiao).HasMaxLength(250).IsRequired(true);
                tbl.Property(it => it.Description).HasMaxLength(500).IsRequired(false);
            });
            builder.Entity<KhuVuc>(tbl =>
            {
                tbl.ToTable("KhuVucModel", "tMasterData");
            });
            builder.Entity<HeDaoTao>(tbl =>
            {
                tbl.ToTable("HeDaoTaoModel", "tMasterData");

            });
            builder.Entity<HocHam>(tbl =>
            {
                tbl.ToTable("HocHamModel", "tMasterData");
                tbl.HasKey(it => it.MaHocHam);
                tbl.Property(it => it.MaHocHam).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenHocHam).HasMaxLength(250).IsRequired(true);
            });
            builder.Entity<TinhThanhPho>(tbl =>
            {
                tbl.ToTable("TinhThanhPhoModel", "tMasterData");
                tbl.HasKey(it => it.MaTinhThanhPho);
                tbl.Property(it => it.MaTinhThanhPho).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenTinhThanhPho).HasMaxLength(250).IsRequired(true);
                tbl.Property(it => it.MaKhuVuc).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.Description).HasMaxLength(500).IsRequired(false);
                tbl.HasOne<KhuVuc>(it => it.KhuVuc)
                .WithMany(it => it.TinhThanhPhos)
                .HasForeignKey(it => it.MaKhuVuc)
                .HasConstraintName("FK_TinhThanhPho_KhuVuc");
            });
            builder.Entity<QuanHuyen>(tbl =>
            {
                tbl.ToTable("QuanHuyenModel", "tMasterData");
                tbl.HasKey(it => it.MaQuanHuyen);
                tbl.Property(it => it.MaQuanHuyen).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.MaTinhThanhPho).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenQuanHuyen).HasMaxLength(250).IsRequired(true);
                tbl.Property(it => it.Description).HasMaxLength(500).IsRequired(false);
                tbl.HasOne<TinhThanhPho>(it => it.TinhThanhPho)
                .WithMany(it => it.QuanHuyens)
                .HasForeignKey(it => it.MaTinhThanhPho)
                .HasConstraintName("FK_QuanHuyen_TinhThanhPho");
            });
            builder.Entity<PhuongXa>(tbl =>
            {
                tbl.ToTable("PhuongXaModel", "tMasterData");
                tbl.HasKey(it => it.MaPhuongXa);
                tbl.Property(it => it.MaPhuongXa).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.MaQuanHuyen).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenPhuongXa).HasMaxLength(250).IsRequired(true);
                tbl.Property(it => it.Description).HasMaxLength(500).IsRequired(false);
                tbl.HasOne<QuanHuyen>(it => it.QuanHuyen)
                .WithMany(it => it.PhuongXas)
                .HasForeignKey(it => it.MaQuanHuyen)
                .HasConstraintName("FK_PhuongXa_QuanHuyen");
            });
            builder.Entity<TinhTrang>(tbl =>
            {
                tbl.ToTable("TinhTrangModel", "tMasterData");
                tbl.HasKey(it => it.MaTinhTrang);
                tbl.Property(it => it.MaTinhTrang).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenTinhTrang).HasMaxLength(250).IsRequired(true);
                tbl.Property(it => it.Description).HasMaxLength(500).IsRequired(false);
            });
            builder.Entity<HinhThucKyLuat>(tbl =>
            {
                tbl.ToTable("HinhThucKyLuat", "tMasterData");
                tbl.HasKey(it => it.MaHinhThucKyLuat);
                tbl.Property(it => it.MaHinhThucKyLuat).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenHinhThucKyLuat).HasMaxLength(250).IsRequired(true);;
            });
            #endregion Master data
            #region NhanSu
            builder.Entity<CanBo>(entity =>
            {
                entity.ToTable("CanBo", "NS");
                entity.HasKey(it => it.IDCanBo);
                entity.Property(it => it.MaCanBo).IsUnicode(true).IsRequired(true).HasMaxLength(20);

                entity.HasOne<TinhTrang>(it => it.TinhTrang)
                    .WithMany(it => it.CanBos)
                    .HasForeignKey(it => it.MaTinhTrang)
                    .HasConstraintName("FKg_CanBo_TinhTrang");

                entity.HasOne<CoSo>(it => it.CoSo)
                    .WithMany(it => it.CanBos)
                    .HasForeignKey(it => it.IdCoSo)
                    .HasConstraintName("FK_CanBo_CoSo");

                entity.HasOne<Department>(it => it.Department)
                  .WithMany(it => it.CanBos)
                  .HasForeignKey(it => it.IdDepartment)
                  .HasConstraintName("FK_CanBo_Department");

                entity.HasOne<ChucVu>(it => it.ChucVu)
                 .WithMany(it => it.CanBos)
                 .HasForeignKey(it => it.MaChucVu)
                 .HasConstraintName("FK_CanBo_ChucVu");

                entity.HasOne<BacLuong>(it => it.BacLuong)
                 .WithMany(it => it.CanBos)
                 .HasForeignKey(it => it.MaBacLuong)
                 .HasConstraintName("FK_CanBo_BacLuong");

                entity.HasOne<TrinhDoHocVan>(it => it.TrinhDoHocVan)
                .WithMany(it => it.CanBos)
                .HasForeignKey(it => it.MaTrinhDoHocVan)
                .HasConstraintName("FK_CanBo_TrinhDoHocVan");

                entity.HasOne<TrinhDoTinHoc>(it => it.TrinhDoTinHoc)
               .WithMany(it => it.CanBos)
               .HasForeignKey(it => it.MaTrinhDoTinHoc)
               .HasConstraintName("FK_CanBo_TrinhDoTinHoc");

                entity.HasOne<TrinhDoNgoaiNgu>(it => it.TrinhDoNgoaiNgu)
               .WithMany(it => it.CanBos)
               .HasForeignKey(it => it.MaTrinhDoNgoaiNgu)
               .HasConstraintName("FK_CanBo_TrinhDoNgoaiNgu");

                entity.HasOne<TrinhDoChinhTri>(it => it.TrinhDoChinhTri)
               .WithMany(it => it.CanBos)
               .HasForeignKey(it => it.MaTrinhDoChinhTri)
               .HasConstraintName("FK_CanBo_TrinhDoChinhTri");

                entity.HasOne<DanToc>(it => it.DanToc)
                 .WithMany(it => it.CanBos)
                 .HasForeignKey(it => it.MaDanToc)
                 .HasConstraintName("FK_CanBo_DanToc");

                entity.HasOne<TonGiao>(it => it.TonGiao)
                 .WithMany(it => it.CanBos)
                 .HasForeignKey(it => it.MaTonGiao)
                 .HasConstraintName("FK_CanBo_TonGiao");

            entity.HasOne<PhanHe>(it => it.PhanHe)
               .WithMany(it => it.CanBos)
               .HasForeignKey(it => it.MaPhanHe)
               .HasConstraintName("FK_CanBo_PhanHe");
            entity.HasOne<HocHam>(it => it.HocHam)
              .WithMany(it => it.CanBos)
              .HasForeignKey(it => it.MaHocHam)
              .HasConstraintName("FK_CanBo_HocHam");
             entity.HasOne<HeDaoTao>(it => it.HeDaoTao)
             .WithMany(it => it.CanBos)
             .HasForeignKey(it => it.MaHeDaoTao)
             .HasConstraintName("FK_CanBo_HeDaoTao");
            });
            builder.Entity<QuanHeGiaDinh>(entity => {
                entity.ToTable("QuanHeGiaDinh", "NS");
                entity.HasKey(it => it.IDQuanheGiaDinh);
                entity.Property(it => it.HoTen).HasMaxLength(150).IsRequired(true);
                entity.Property(it => it.NgaySinh).IsRequired(false);
                entity.Property(it => it.NgheNghiep).HasMaxLength(150).IsRequired(false);
                entity.Property(it => it.NoiLamVien).HasMaxLength(250).IsRequired(false);
                entity.Property(it => it.DiaChi).HasMaxLength(250).IsRequired(false);
                entity.Property(it => it.GhiChu).HasMaxLength(250).IsRequired(false);


                entity.HasOne<CanBo>(it=>it.CanBo)
                .WithMany(it=>it.QuanHeGiaDinhs)
                .HasForeignKey(it=>it.IDCanBo)
                .HasConstraintName("FK_QuanHeGiaDinh_CanBo");

                entity.HasOne<LoaiQuanHeGiaDinh>(it => it.LoaiQuanhe)
                .WithMany(it => it.QuanHeGiaDinhs)
                .HasForeignKey(it => it.IDLoaiQuanHeGiaDinh)
                .HasConstraintName("FK_QuanHeGiaDinh_LoaiQuanHeGiaDinh");

            });
            builder.Entity<QuaTrinhKhenThuong>(entity => {
                entity.ToTable("QuaTrinhKhenThuong", "NS");
                entity.HasKey(it => it.IDQuaTrinhKhenThuong);
                entity.Property(it => it.SoQuyetDinh).HasMaxLength(50).IsRequired(true);
                entity.Property(it => it.NguoiKy).HasMaxLength(150).IsRequired(false);
                entity.Property(it => it.LyDo).HasMaxLength(250).IsRequired(false);
                entity.Property(it => it.GhiChu).HasMaxLength(550).IsRequired(false);


                entity.HasOne<CanBo>(it => it.CanBo)
                .WithMany(it => it.QuaTrinhKhenThuongs)
                .HasForeignKey(it => it.IDCanBo)
                .HasConstraintName("FK_QuaTrinhKhenThuong_CanBo");

                entity.HasOne<HinhThucKhenThuong>(it => it.HinhThucKhenThuong)
                .WithMany(it => it.QuaTrinhKhenThuong)
                .HasForeignKey(it => it.MaHinhThucKhenThuong)
                .HasConstraintName("FK_QuanHeGiaDinh_HinhThucKhenThuong");

                entity.HasOne<DanhHieuKhenThuong>(it => it.DanhHieuKhenThuong)
                .WithMany(it => it.QuaTrinhKhenThuong)
                .HasForeignKey(it => it.MaDanhHieuKhenThuong)
                .HasConstraintName("FK_QuanHeGiaDinh_DanhHieuKhenThuong");

            });
            builder.Entity<QuaTrinhKyLuat>(entity => {
                entity.ToTable("QuaTrinhKyLuat", "NS");
                entity.HasKey(it => it.IdQuaTrinhKyLuat);
                entity.Property(it => it.SoQuyetDinh).HasMaxLength(50).IsRequired(true);
                entity.Property(it => it.NgayKy).IsRequired(true);
                entity.Property(it => it.NguoiKy).HasMaxLength(150).IsRequired(true);
                entity.Property(it => it.LyDo).HasMaxLength(250).IsRequired(true);
                entity.Property(it => it.GhiChu).HasMaxLength(550).IsRequired(false);


                entity.HasOne<CanBo>(it => it.CanBo)
                .WithMany(it => it.QuaTrinhKyLuats)
                .HasForeignKey(it => it.IDCanBo)
                .HasConstraintName("FK_QuaTrinhKyLuat_CanBo");

                entity.HasOne<HinhThucKyLuat>(it => it.HinhThucKyLuat)
                .WithMany(it => it.QuaTrinhKyLuats)
                .HasForeignKey(it => it.MaHinhThucKyLuat)
                .HasConstraintName("FK_QuaTrinhKyLuat_HinhThucKyLuat");


            });
            builder.Entity<QuaTrinhDaoTao>(entity => {
                entity.ToTable("QuaTrinhDaoTao", "NS");
                entity.HasKey(it => it.IDQuaTrinhDaoTao);
                entity.Property(it => it.MaChuyenNganh).HasMaxLength(50).IsRequired(true);
                entity.Property(it => it.MaLoaiBangCap).HasMaxLength(50).IsRequired(true);
                entity.Property(it => it.MaHinhThucDaoTao).HasMaxLength(50).IsRequired(true);
                entity.Property(it => it.CoSoDaoTao).HasMaxLength(250).IsRequired(true);
                entity.Property(it => it.QuocGia).HasMaxLength(150).IsRequired(true);
                entity.Property(it => it.NgayTotNghiep).HasMaxLength(10).IsRequired(true);
                entity.Property(it => it.FileDinhKem).HasMaxLength(500).IsRequired(false);
                entity.Property(it => it.GhiChu).HasMaxLength(500).IsRequired(false);
                entity.Property(it => it.LuanAnTN).IsRequired(false);


                entity.HasOne<CanBo>(it => it.CanBo)
                .WithMany(it => it.QuaTrinhDaoTaos)
                .HasForeignKey(it => it.IDCanBo)
                .HasConstraintName("FK_QuaTrinhDaoTao_CanBo");

                entity.HasOne<LoaiBangCap>(it => it.LoaiBangCap)
                .WithMany(it => it.QuaTrinhDaoTaos)
                .HasForeignKey(it => it.MaLoaiBangCap)
                .HasConstraintName("FK_QuaTrinhDaoTao_LoaiBangCap");

                entity.HasOne<ChuyenNganh>(it => it.ChuyenNganh)
               .WithMany(it => it.QuaTrinhDaoTaos)
               .HasForeignKey(it => it.MaChuyenNganh)
               .HasConstraintName("FK_QuaTrinhDaoTao_ChuyenNganh");

                entity.HasOne<HinhThucDaoTao>(it => it.HinhThucDaoTao)
              .WithMany(it => it.QuaTrinhDaoTaos)
              .HasForeignKey(it => it.MaHinhThucDaoTao)
              .HasConstraintName("FK_QuaTrinhDaoTao_HinhThucDaoTao");


            });
            #endregion NhanSu

            #region Permission
            builder.Entity<Account>(entity =>
            {
                entity.ToTable("Account", "pms");
                entity.HasKey(it => it.AccountId);
                entity.Property(it => it.UserName).HasMaxLength(150).IsRequired();
                entity.Property(it => it.Password).HasMaxLength(250).IsRequired();
                entity.Property(it => it.FullName).HasMaxLength(500).IsRequired();
                entity.Property(it => it.CreatedTime).HasDefaultValueSql<DateTime>("GETDATE()");
                entity.Property(it => it.Actived).HasDefaultValue(true);
                entity.Property(it => it.CreatedAccountId).IsRequired(false);
                entity.HasMany<AccountInRoleModel>(it => it.AccountInRoleModels)
                .WithOne(it => it.Account).OnDelete(DeleteBehavior.ClientCascade);
            });
            builder.Entity<RolesModel>(entity =>
            {
                entity.ToTable("RolesModel", "pms");
                entity.HasKey(it => it.RolesId);
                entity.Property(it => it.RolesCode).IsRequired().HasMaxLength(250);
                entity.Property(it => it.RolesName).IsRequired().HasMaxLength(500);
                entity.Property(it => it.CreatedTime).HasDefaultValueSql("GETDATE()");
                entity.Property(it => it.Actived).HasDefaultValue(false);
            });
            builder.Entity<FunctionModel>(entity =>
            {
                entity.ToTable("FunctionModel", "pms");
                entity.Property(it => it.OrderIndex).HasDefaultValue(0);
                entity.HasKey(it => it.FunctionId);
            });
            builder.Entity<AccountInRoleModel>(entity =>
            {
                entity.ToTable("AccountInRoleModel", "pms");
                entity.HasKey(it => new { it.AccountId, it.RolesId });
                entity.HasOne<Account>(it => it.Account)
                    .WithMany(it => it.AccountInRoleModels)
                    .HasForeignKey(it => it.AccountId)
                    .HasConstraintName("FK_AccountInRoleModel_Account");

                entity.HasOne<RolesModel>(it => it.RolesModel)
                 .WithMany(it => it.AccountInRoleModels)
                 .HasForeignKey(it => it.RolesId)
                 .HasConstraintName("FK_AccountInRoleModel_RolesModel");
            });
            builder.Entity<MenuModel>(entity =>
            {

                entity.ToTable("MenuModel", "pms");
                entity.HasKey(it => it.MenuId);
                entity.Property(it => it.MenuCode).HasMaxLength(100).IsRequired();
                entity.Property(it => it.MenuName).HasMaxLength(500).IsRequired().IsUnicode(true);
                entity.Property(it => it.OrderIndex).IsRequired();
                entity.Property(it => it.Icon).HasMaxLength(150);
                entity.Property(it => it.Actived).HasDefaultValue(true);
                entity.Property(it => it.Href).HasMaxLength(500).HasDefaultValue("#");
                entity.HasMany<MenuModel>(it => it.Children);

            });
            builder.Entity<HistoryModel>(entity =>
            {
                entity.ToTable("HistoryModel", "utilities");
            });
            builder.Entity<PageFunctionModel>(entity =>
            {
                entity.ToTable("PageFunctionModel", "pms");
                entity.HasKey(it => new { it.MenuId, it.FunctionId });
                entity.HasOne(it => it.FunctionModel).WithMany(g => g.PageFunctionModels)
                .HasForeignKey(it => it.FunctionId).HasConstraintName("FK_PageFunctionModel_FunctionModel");
                entity.HasOne(it => it.MenuModel).WithMany(g => g.PageFunctionModels)
               .HasForeignKey(it => it.MenuId).HasConstraintName("FK_PageFunctionModel_MenuModel");
            });
            builder.Entity<PagePermissionModel>(entity =>
            {
                entity.ToTable("PagePermissionModel", "pms");
                entity.HasKey(it => new { it.MenuId, it.FunctionId, it.RolesId });

                entity.HasOne(it => it.Roles).WithMany(it => it.PagePermissionModels)
                      .HasForeignKey(it => it.RolesId).HasConstraintName("FK_PagePermissionModel_Roles");

                entity.HasOne(it => it.Function).WithMany(it => it.PagePermissionModels)
                      .HasForeignKey(it => it.FunctionId).HasConstraintName("FK_PagePermissionModel_Function").OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(it => it.Menu).WithMany(it => it.PagePermissionModels)
                      .HasForeignKey(it => it.MenuId).HasConstraintName("FK_PagePermissionModel_Menu").OnDelete(DeleteBehavior.NoAction);

            });
            #endregion Permission
            // Permission

        }
    }
}
