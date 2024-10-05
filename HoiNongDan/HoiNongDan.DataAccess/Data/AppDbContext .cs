using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using HoiNongDan.Models;
using HoiNongDan.Models.Entitys;
using HoiNongDan.Models.Entitys.MasterData;
using HoiNongDan.Models.Entitys.NhanSu;
using System.ComponentModel.DataAnnotations.Schema;
using HoiNongDan.Models.Entitys.HoiVien;

namespace HoiNongDan.DataAccess
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
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<PhamVi> PhamVis { get; set; }
        public DbSet<Author_Token> Author_Tokens { get; set; }
        public DbSet<UsageLog> UsageLogs { get; set; }

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
        public DbSet<HocVi> HocVis { get; set; }
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
        public DbSet<FileDinhKem> FileDinhKems { get; set; }
        public DbSet<LoaiDinhKem> LoaiDinhKems { get; set; }
        public DbSet<NgheNghiep> NgheNghieps { get; set; }
        public DbSet<GiaDinhThuocDien> GiaDinhThuocDiens { get; set; }
        public DbSet<TrinhDoChuyenMon> TrinhDoChuyenMons { get; set; }
        public DbSet<ChiHoi> ChiHois { get; set; }
        public DbSet<ToHoi> ToHois { get; set; }
        public DbSet<NguonVon> NguonVons { get; set; }
        public DbSet<HinhThucHoTro> HinhThucHoTros { get; set; }
        public DbSet<DoanTheChinhTri_HoiDoan> DoanTheChinhTri_HoiDoans { get; set; }
        
        public DbSet<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac> CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTacs { get; set; }
        public DbSet<ToHoiNganhNghe_ChiHoiNganhNghe> ToHoiNganhNghe_ChiHoiNganhNghes { get; set; }
        public DbSet<Dot> Dots { get; set; }
        public DbSet<LopHoc> LopHocs { get; set; }
    
        #endregion
        #region nhanSu
        public DbSet<CanBo> CanBos { get; set; }
        public DbSet<CanBoQuaTrinhLuong> CanBoQuaTrinhLuongs { get; set; }
        public DbSet<BaoCaoThucLucHoi> BaoCaoThucLucHois { get; set; }
        public DbSet<QuanHeGiaDinh> QuanHeGiaDinhs { get; set; }
        public DbSet<QuaTrinhKhenThuong> QuaTrinhKhenThuongs { get; set; }
        public DbSet<QuaTrinhKyLuat> QuaTrinhKyLuats { get; set; }

        public DbSet<QuaTrinhBoNhiem> QuaTrinhBoNhiems { get; set; }
        public DbSet<QuaTrinhCongTac> QuaTrinhCongTacs { get; set; }
        public DbSet<QuaTrinhMienNhiem> QuaTrinhMienNhiems { get; set; }
        public DbSet<DaoTaoBoiDuong> DaoTaoBoiDuongs { get; set; }
        public DbSet<HuuTri> HuuTris { get; set; }
        #endregion nhanSu

        #region Hội viên
        public DbSet<DiaBanHoatDong> DiaBanHoatDongs { get; set; }
        public DbSet<DanhGiaHoiVien> DanhGiaHoiViens { get; set; }
        public DbSet<DanhGiaToChucHoi> DanhGiaToChucHois { get; set; }

        public DbSet<HoiVienHoTro> HoiVienHoTros { get; set; }
        public DbSet<HoiVienHoiDap> HoiVienHoiDaps { get; set; }
        public DbSet<DonVi> DonVis { get; set; }
        public DbSet<DoanTheChinhTri_HoiDoan_HoiVien> DoanTheChinhTri_HoiDoan_HoiViens { get; set; }
        public DbSet<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien> CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens { get; set; }
        public DbSet<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien> ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens { get; set; }
        public DbSet<LichSinhHoatChiToHoi> LichSinhHoatChiToHois { get; set; }
        public DbSet<LichSinhHoatChiToHoi_NguoiThamGia> LichSinhHoatChiToHoi_NguoiThamGias { get; set; }
        public DbSet<PhatTrienDang> PhatTrienDangs { get; set; }
        public DbSet<PhatTrienDang_HoiVien> PhatTrienDang_HoiViens { get; set; }
        public DbSet<HoiVienCapThe> HoiVienCapThes { get; set; }
        public DbSet<VayVon> VayVons { get; set; }
        public DbSet<HoiVienLichSuDuyet> HoiVienLichSuDuyets { get; set; }
        public DbSet<ThongKeBien_ToChucHoi_HoiVien> ThongKeBien_ToChucHoi_HoiViens { get; set; }
        public DbSet<BaoCaoThucLucHoiNam> BaoCaoThucLucHoiNams { get; set; }
        public DbSet<HoiVien_ChiHoi> HoiVien_ChiHois { get; set; }
        #endregion Hội viên


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            #region Proc
            builder.Entity<CanBoDenTuoiNghiHuuVM>().HasNoKey();
            #endregion Proc
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
            builder.Entity<Dot>(tbl =>
            {
                tbl.ToTable("Dot", "tMasterData");
                tbl.HasKey(it => it.MaDot);
            });
            builder.Entity<HinhThucHoTro>(tbl =>
            {
                tbl.ToTable("HinhThucHoTro", "tMasterData");
                tbl.HasKey(it => it.MaHinhThucHoTro);
                tbl.Property(it => it.TenHinhThuc).IsRequired()
                   .HasMaxLength(500);
            });
            builder.Entity<NguonVon>(tbl =>
            {
                tbl.ToTable("NguonVon", "tMasterData");
                tbl.HasKey(it => it.MaNguonVon);
                tbl.Property(it => it.TenNguonVon).IsRequired()
                   .HasMaxLength(500);
            });
            builder.Entity<ChiHoi>(tbl =>
            {
                tbl.ToTable("ChiHoi", "tMasterData");
                tbl.HasKey(it => it.MaChiHoi);
                tbl.Property(it => it.TenChiHoi).IsRequired()
                   .HasMaxLength(500);
            });
            builder.Entity<ToHoi>(tbl =>
            {
                tbl.ToTable("ToHoi", "tMasterData");
                tbl.HasKey(it => it.MaToHoi);
                tbl.Property(it => it.TenToHoi).IsRequired()
                   .HasMaxLength(500);
            });
            builder.Entity<TrinhDoChuyenMon>(tbl =>
            {
                tbl.ToTable("TrinhDoChuyenMon", "tMasterData");
                tbl.HasKey(it => it.MaTrinhDoChuyenMon);
                tbl.Property(it => it.TenTrinhDoChuyenMon).IsRequired()
                   .HasMaxLength(200);
            });
            builder.Entity<LoaiDinhKem>(tbl =>
            {
                tbl.ToTable("LoaiDinhKem", "tMasterData");
            });
            builder.Entity<NgheNghiep>(tbl =>
            {
                tbl.ToTable("NgheNghiep", "tMasterData");
            });
            builder.Entity<GiaDinhThuocDien>(tbl =>
            {
                tbl.ToTable("GiaDinhThuocDien", "tMasterData");
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
            builder.Entity<HocVi>(tbl =>
            {
                tbl.ToTable("HocViModel", "tMasterData");
                tbl.HasKey(it => it.MaHocVi);
                tbl.Property(it => it.MaHocVi).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenHocVi).HasMaxLength(250).IsRequired(true);
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
            builder.Entity<DoanTheChinhTri_HoiDoan>(tbl =>
            {
                tbl.ToTable("DoanTheChinhTri_HoiDoan", "tMasterData");
                tbl.HasKey(it => it.MaDoanTheChinhTri_HoiDoan);
                tbl.Property(it => it.TenDoanTheChinhTri_HoiDoan).HasMaxLength(500).IsRequired(true); ;
            });
            builder.Entity<DoanTheChinhTri_HoiDoan>(tbl =>
            {
                tbl.ToTable("DoanTheChinhTri_HoiDoan", "tMasterData");
                tbl.HasKey(it => it.MaDoanTheChinhTri_HoiDoan);
                tbl.Property(it => it.TenDoanTheChinhTri_HoiDoan).HasMaxLength(500).IsRequired(true); ;
            });
            builder.Entity<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac>(tbl =>
            {
                tbl.ToTable("CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac", "tMasterData");
                tbl.HasKey(it => it.Id_CLB_DN_MH_HTX_THT);
                tbl.Property(it => it.Ten).HasMaxLength(500).IsRequired(true);
                tbl.Property(it => it.Loai).HasMaxLength(50).IsRequired(false);
            });
            builder.Entity<ToHoiNganhNghe_ChiHoiNganhNghe>(tbl =>
            {
                tbl.ToTable("ToHoiNganhNghe_ChiHoiNganhNghe", "tMasterData");
                tbl.HasKey(it => it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe);
                tbl.Property(it => it.Ten).HasMaxLength(500).IsRequired(true); ;
            });

            builder.Entity<LopHoc>(tbl =>
            {
                tbl.ToTable("LopHoc", "tMasterData");
                tbl.HasKey(it => it.IDLopHoc);
                tbl.Property(it => it.TenLopHoc).HasMaxLength(500).IsRequired(true);

                tbl.HasOne<HinhThucHoTro>(it => it.HinhThucHoTro)
                .WithMany(it => it.LopHocs)
                .HasForeignKey(it => it.MaHinhThucHoTro)
                .HasConstraintName("FK_LopHoc_HinhThucHoTro");

            });
            #endregion Master data
            #region NhanSu
            builder.Entity<CanBo>(entity =>
            {
                entity.ToTable("CanBo", "NS");
                entity.HasKey(it => it.IDCanBo);
                entity.Property(it => it.MaCanBo).IsUnicode(true).IsRequired(false).HasMaxLength(100);
                entity.Property(it => it.MaDinhDanh).IsUnicode(true).IsRequired(false).HasMaxLength(100);
                entity.HasOne<TinhTrang>(it => it.TinhTrang)
                    .WithMany(it => it.CanBos)
                    .HasForeignKey(it => it.MaTinhTrang)
                    .HasConstraintName("FKg_CanBo_TinhTrang");

                entity.HasOne<QuanHuyen>(it => it.QuanHuyen)
                   .WithMany(it => it.CanBos)
                   .HasForeignKey(it => it.MaQuanHuyen)
                   .HasConstraintName("FK_CanBo_QuanHuyenModel");


                entity.HasOne<CoSo>(it => it.CoSo)
                    .WithMany(it => it.CanBos)
                    .HasForeignKey(it => it.IdCoSo)
                    .HasConstraintName("FK_CanBo_CoSo")
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne<Department>(it => it.Department)
                  .WithMany(it => it.CanBos)
                  .HasForeignKey(it => it.IdDepartment)
                  .HasConstraintName("FK_CanBo_Department");

                entity.HasOne<DiaBanHoatDong>(it => it.DiaBanHoatDong)
                .WithMany(it => it.CanBos)
                .HasForeignKey(it => it.MaDiaBanHoatDong)
                .HasConstraintName("FK_CanBo_DiaBanHoatDong");

                entity.HasOne<NgheNghiep>(it => it.NgheNghiep)
                   .WithMany(it => it.CanBos)
                   .HasForeignKey(it => it.MaNgheNghiep)
                   .HasConstraintName("FK_CanBo_NgheNghiep");


                entity.HasOne<NgheNghiep>(it => it.NgheNghiep)
                  .WithMany(it => it.CanBos)
                  .HasForeignKey(it => it.MaNgheNghiep)
                  .HasConstraintName("FK_CanBo_NgheNghiep");

                entity.HasOne<GiaDinhThuocDien>(it => it.GiaDinhThuocDien)
                 .WithMany(it => it.CanBos)
                 .HasForeignKey(it => it.MaGiaDinhThuocDien)
                 .HasConstraintName("FK_CanBo_GiaDinhThuocDien");

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

                entity.HasOne<TrinhDoChuyenMon>(it => it.TrinhDoChuyenMon)
               .WithMany(it => it.CanBos)
               .HasForeignKey(it => it.MaTrinhDoChuyenMon)
               .HasConstraintName("FK_CanBo_TrinhDoChuyenMon");

                entity.HasOne<HocVi>(it => it.HocVi)
                  .WithMany(it => it.CanBos)
                  .HasForeignKey(it => it.MaHocVi)
                  .HasConstraintName("FK_CanBo_HocVi");

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

                entity.HasOne<ChiHoi>(it => it.ChiHoi)
                .WithMany(it => it.CanBos)
                .HasForeignKey(it => it.MaChiHoi)
                .HasConstraintName("FK_CanBo_ChiHoi");

                entity.HasOne<ToHoi>(it => it.ToHoi)
                .WithMany(it => it.CanBos)
                .HasForeignKey(it => it.MaToHoi)
                .HasConstraintName("FK_CanBo_ToHoi");

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
             entity.HasOne<HocVi>(it => it.HocVi)
                      .WithMany(it => it.CanBos)
                      .HasForeignKey(it => it.MaHocVi)
                      .HasConstraintName("FK_CanBo_HocVi");

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

                entity.HasOne<CanBo>(it => it.HoiVien)
               .WithMany(it => it.HVQuanHeGiaDinhs)
               .HasForeignKey(it => it.IDHoiVien)
               .HasConstraintName("FK_QuanHeGiaDinh_HoiVien");

                entity.HasOne<LoaiQuanHeGiaDinh>(it => it.LoaiQuanhe)
                .WithMany(it => it.QuanHeGiaDinhs)
                .HasForeignKey(it => it.IDLoaiQuanHeGiaDinh)
                .HasConstraintName("FK_QuanHeGiaDinh_LoaiQuanHeGiaDinh");

            });
            builder.Entity<CanBoQuaTrinhLuong>(entity => {
                entity.ToTable("CanBoQuaTrinhLuong", "NS");
                entity.HasKey(it => it.ID);
              
                entity.HasOne<CanBo>(it => it.CanBo)
                .WithMany(it => it.CanBoQuaTrinhLuongs)
                .HasForeignKey(it => it.IDCanBo)
                .HasConstraintName("FK_CanBoQuaTrinhLuong_CanBo");

            });
            builder.Entity<QuaTrinhKhenThuong>(entity => {
                entity.ToTable("QuaTrinhKhenThuong", "NS");
                entity.HasKey(it => it.IDQuaTrinhKhenThuong);
                entity.Property(it => it.SoQuyetDinh).HasMaxLength(50).IsRequired(false);
                entity.Property(it => it.NguoiKy).HasMaxLength(150).IsRequired(false);
                entity.Property(it => it.NoiDung).HasMaxLength(250).IsRequired(false);
                entity.Property(it => it.Loai).HasMaxLength(20).IsRequired(false);
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
            builder.Entity<DaoTaoBoiDuong>(entity => {
                entity.ToTable("DaoTaoBoiDuong", "NS");
                entity.HasKey(it => it.Id);
                entity.Property(it => it.TuNgay).IsRequired(false);
                entity.Property(it => it.DenNgay).IsRequired(false);
                entity.Property(it => it.NoiDungDaoTao).IsRequired(true).HasMaxLength(1000);
                entity.Property(it => it.GhiChu).IsRequired(false).HasMaxLength(500);
                entity.HasOne<HinhThucDaoTao>(it => it.HinhThucDaoTao)
                .WithMany(it => it.DaoTaoBoiDuongs)
                .HasForeignKey(it => it.MaHinhThucDaoTao)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_DaoTaoBoiDuongc_HinhThucDaoTao");
                entity.HasOne<LoaiBangCap>(it => it.LoaiBangCap)
                .WithMany(it => it.DaoTaoBoiDuongs)
                .HasForeignKey(it => it.MaLoaiBangCap)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_DaoTaoBoiDuong_MaLoaiBangCap");

                entity.HasOne<CanBo>(it => it.CanBo)
                .WithMany(it => it.DaoTaoBoiDuongs)
                .HasForeignKey(it => it.IDCanBo)
                .HasConstraintName("FK_DaoTaoBoiDuong_CanBo");
            });
            builder.Entity<QuaTrinhCongTac>(entity => {
                entity.ToTable("QuaTrinhCongTac", "NS");
                entity.HasKey(it => it.IDQuaTrinhCongTac);
                entity.Property(it => it.GhiChu).IsRequired(false).HasMaxLength(500);
                entity.HasOne<ChucVu>(it => it.ChucVu)
                .WithMany(it => it.QuaTrinhCongTacs)
                .HasForeignKey(it => it.MaChucVu)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_QuaTrinhCongTac_ChucVu");

                entity.HasOne<CanBo>(it => it.CanBo)
                .WithMany(it => it.QuaTrinhCongTacs)
                .HasForeignKey(it => it.IDCanBo)
                .HasConstraintName("FK_QuaTrinhCongTac_CanBo");
            });
            builder.Entity<QuaTrinhBoNhiem>(entity => {
                entity.ToTable("QuaTrinhBoNhiem", "NS");
                entity.HasKey(it => it.IdQuaTrinhBoNhiem);
                entity.Property(it => it.NgayQuyetDinh).IsRequired(true);
                entity.Property(it => it.SoQuyetDinh).HasMaxLength(100).IsRequired(true);
                entity.Property(it => it.NguoiKy).HasMaxLength(150).IsRequired(true);
                entity.Property(it => it.GhiChu).HasMaxLength(500).IsRequired(false);

               entity.HasOne<CanBo>(it => it.CanBo)
               .WithMany(it => it.QuaTrinhBoNhiems)
               .HasForeignKey(it => it.IDCanBo)
               .HasConstraintName("FK_QuaTrinhBoNhiem_CanBo");

               entity.HasOne<ChucVu>(it => it.ChucVu)
               .WithMany(it => it.QuaTrinhBoNhiems)
               .HasForeignKey(it => it.MaChucVu)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_QuaTrinhBoNhiem_ChucVu");

              entity.HasOne<CoSo>(it => it.CoSo)
               .WithMany(it => it.QuaTrinhBoNhiems)
               .HasForeignKey(it => it.IdCoSo)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_QuaTrinhBoNhiem_CoSo");

              entity.HasOne<Department>(it => it.Department)
               .WithMany(it => it.QuaTrinhBoNhiems)
               .HasForeignKey(it => it.IdDepartment)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_QuaTrinhBoNhiem_Department");
            });
            builder.Entity<QuaTrinhMienNhiem>(entity => {
                entity.ToTable("QuaTrinhMienNhiem", "NS");
                entity.HasKey(it => it.IDQuaTrinhMienNhiem);
                entity.Property(it => it.NgayQuyetDinh).IsRequired(true);
                entity.Property(it => it.SoQuyetDinh).HasMaxLength(100).IsRequired(true);
                entity.Property(it => it.NguoiKy).HasMaxLength(150).IsRequired(true);
                entity.Property(it => it.GhiChu).HasMaxLength(500).IsRequired(false);

                entity.HasOne<CanBo>(it => it.CanBo)
                .WithMany(it => it.QuaTrinhMienNhiems)
                .HasForeignKey(it => it.IDCanBo)
                .HasConstraintName("FK_QuaTrinhMienNhiem_CanBo");

                entity.HasOne<ChucVu>(it => it.ChucVu)
                .WithMany(it => it.QuaTrinhMienNhiems)
                .HasForeignKey(it => it.MaChucVu)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_QuaTrinhMienNhiem_ChucVu");

                entity.HasOne<CoSo>(it => it.CoSo)
                 .WithMany(it => it.QuaTrinhMienNhiems)
                 .HasForeignKey(it => it.IdCoSo)
                 .OnDelete(DeleteBehavior.NoAction)
                 .HasConstraintName("FK_QuaTrinhMienNhiem_CoSo");

                entity.HasOne<Department>(it => it.Department)
                 .WithMany(it => it.QuaTrinhMienNhiems)
                 .HasForeignKey(it => it.IdDepartment)
                 .OnDelete(DeleteBehavior.NoAction)
                 .HasConstraintName("FK_QuaTrinhMienNhiem_Department");
            });
            builder.Entity<FileDinhKem>(tbl =>
            {
                tbl.HasKey(it => it.Key);
                tbl.ToTable("FileDinhKem", "NS");
                tbl.Property(it => it.Url).IsRequired()
                   .HasMaxLength(500);
                tbl.Property(it => it.FileName).IsRequired()
                  .HasMaxLength(200);

                tbl.HasOne<LoaiDinhKem>(it => it.LoaiDinhKem)
                  .WithMany(it => it.FileDinhKems)
                  .HasForeignKey(it => it.IDLoaiDinhKem)
                  .OnDelete(DeleteBehavior.NoAction)
                  .HasConstraintName("FK_FileDinhKem_LoaiDinhKem");

                tbl.HasOne<CanBo>(it => it.CanBo)
                  .WithMany(it => it.FileDinhKems)
                  .HasForeignKey(it => it.IdCanBo)
                  .OnDelete(DeleteBehavior.NoAction)
                  .HasConstraintName("FK_FileDinhKem_CanBo");
            });
            builder.Entity<HuuTri>(entity => {
                entity.ToTable("HuuTri", "NS");
                entity.HasKey(it => it.Id);
                entity.Property(it => it.NgayQuyetDinh).IsRequired(true);
                entity.Property(it => it.SoQuyetDinh).HasMaxLength(100).IsRequired(true);
                entity.Property(it => it.NguoiKy).HasMaxLength(150).IsRequired(true);
                entity.Property(it => it.GhiChu).HasMaxLength(500).IsRequired(false);

                entity.HasOne<CanBo>(s => s.CanBo)
               .WithOne(it => it.HuuTri)
               .HasForeignKey<HuuTri>(it => it.IDCanBo)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_HuuTri_CanBo");


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
            builder.Entity<Author_Token>(entity =>
            {
                entity.ToTable("Author_Token", "pms");
                entity.HasKey(it => it.ID);
                entity.Property(it => it.ExpireTimeSpan).IsRequired(true);
                entity.Property(it => it.CreateTime).IsRequired(true);
                entity.HasOne<Account>(it => it.Account)
                    .WithMany(it => it.Author_Tokens)
                    .HasForeignKey(it => it.AccountID)
                    .HasConstraintName("FK_Author_Token_Account");
            });
            builder.Entity<UsageLog>(entity =>
            {
                entity.ToTable("UsageLog", "pms");
                entity.HasKey(it => it.ID);

                entity.HasOne<Account>(it => it.Account)
                    .WithMany(it => it.UsageLogs)
                    .HasForeignKey(it => it.AccountID)
                    .HasConstraintName("FK_UsageLog_Account");
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
            builder.Entity<Parameter>(entity =>
            {
                entity.ToTable("Parameter", "pms");
                entity.HasKey(it => it.ID);
                entity.Property(it => it.Value).IsRequired(false).HasMaxLength(500);
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
            builder.Entity<PhamVi>(tbl =>
            {
                tbl.ToTable("PhamVi", "pms");
                tbl.HasKey(it => new { it.AccountId, it.MaDiabanHoatDong });
                tbl.HasOne<Account>(it => it.Account)
                  .WithMany(it => it.PhamVis)
                  .HasForeignKey(it => it.AccountId)
                  .HasConstraintName("FK_PhamVi_Account");

                tbl.HasOne<DiaBanHoatDong>(it => it.DiaBanHoatDong)
                  .WithMany(it => it.PhamVis)
                  .HasForeignKey(it => it.MaDiabanHoatDong)
                  .HasConstraintName("FK_PhamVi_DiaBanHoatDong");
            });
            #endregion Permission
            // Permission

            #region Hội viên
            builder.Entity<DiaBanHoatDong>(entity => {
                entity.ToTable("DiaBanHoatDong", "HV");
                entity.HasKey(it => it.Id);
                entity.Property(it => it.TenDiaBanHoatDong).IsRequired(true).HasMaxLength(200);
                entity.Property(it => it.DiaChi).IsRequired(true).HasMaxLength(250);
                entity.Property(it => it.GhiChu).IsRequired(false).HasMaxLength(500);
                entity.Property(it => it.MaPhuongXa).IsRequired(false);
                entity.Property(it => it.MaQuanHuyen).IsRequired(false);

                entity.HasOne<TinhThanhPho>(it => it.TinhThanhPho)
                    .WithMany(it => it.DiaBanHoatDongs)
                    .HasForeignKey(it => it.MaTinhThanhPho)
                    .HasConstraintName("FK_DiaBanHoatDong_TinhThanhPho");

                entity.HasOne<QuanHuyen>(it => it.QuanHuyen)
                    .WithMany(it => it.DiaBanHoatDongs)
                    .HasForeignKey(it => it.MaQuanHuyen)
                    .HasConstraintName("FK_DiaBanHoatDong_QuanHuyen");

                entity.HasOne<PhuongXa>(it => it.PhuongXa)
                    .WithMany(it => it.DiaBanHoatDongs)
                    .HasForeignKey(it => it.MaPhuongXa)
                    .HasConstraintName("FK_DiaBanHoatDong_PhuongXa");

            });
            builder.Entity<HoiVienHoiDap>(entity => {
                entity.ToTable("HoiVienHoiDap", "HV");
                entity.HasKey(it => it.ID);
                entity.HasOne<CanBo>(it => it.HoiVien)
                .WithMany(it => it.HoiVienHoiDaps)
                .HasForeignKey(it => it.IDHoivien)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_HoiVienHoiDap_CanBo");

                 entity.HasOne<Account>(it => it.Account)
                .WithMany(it => it.HoiVienHoiDaps)
                .HasForeignKey(it => it.AcountID)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_HoiVienHoiDap_Account");
            });
            builder.Entity<HoiVienHoTro>(entity => {
                entity.ToTable("HoiVienHoTro", "HV");
                entity.HasKey(it => it.ID);
                entity.Property(it => it.NoiDung).IsRequired(true).HasMaxLength(200);
                entity.Property(it => it.GhiChu);


                entity.HasOne<CanBo>(it => it.HoiVien)
                .WithMany(it => it.HoiVienHoTros)
                .HasForeignKey(it => it.IDHoiVien)
                .HasConstraintName("FK_HoiVienVayVon_HoiVien");


                entity.HasOne<LopHoc>(it => it.LopHoc)
               .WithMany(it => it.HoiVienHoTros)
               .HasForeignKey(it => it.IDLopHoc)
               .HasConstraintName("FK_HoiVienVayVon_LopHoc");
            });
            builder.Entity<HoiVienLichSuDuyet>(entity => {
                entity.ToTable("HoiVienLichSuDuyet", "HV");
                entity.HasKey(it => it.ID);

                entity.HasOne<CanBo>(it => it.HoiVien)
                .WithMany(it => it.HoiVienLichSuDuyets)
                .HasForeignKey(it => it.IDHoiVien)
                .HasConstraintName("FK_HoiVienLichSuDuyet_HoiVien");
            });

            builder.Entity<VayVon>(entity => {
                entity.ToTable("VayVon", "HV");
                entity.HasKey(it => it.IDVayVon);
                entity.Property(it => it.NoiDung).IsRequired(true).HasMaxLength(200);
                entity.Property(it => it.SoTienVay);
                entity.Property(it => it.ThoiHangChoVay);
                entity.Property(it => it.LaiSuatVay).HasColumnType("decimal(18,2)");
                entity.Property(it => it.GhiChu);


                entity.HasOne<CanBo>(it => it.HoiVien)
                .WithMany(it => it.VayVons)
                .HasForeignKey(it => it.IDHoiVien)
                .HasConstraintName("FK_VayVons_HoiVien");


                entity.HasOne<NguonVon>(it => it.NguonVon)
                  .WithMany(it => it.VayVons)
                  .HasForeignKey(it => it.MaNguonVon)
                  .HasConstraintName("FK_VayVon_NguonVon");
            });
            builder.Entity<BaoCaoThucLucHoi>(entity => {
                entity.ToTable("BaoCaoThucLucHoi", "HV");
                entity.HasKey(it => it.ID);
                entity.HasOne<DonVi>(it => it.DonVi)
                  .WithMany(it => it.BaoCaoThucLucHois)
                  .HasForeignKey(it => it.IDDonVi)
                  .HasConstraintName("FK_BaoCaoThucLucHoi_DonVi");
            });
            builder.Entity<DanhGiaHoiVien>(entity => {
                entity.ToTable("DanhGiaHoiVien", "HV");
                entity.HasKey(it => it.ID);
                entity.HasOne<CanBo>(it => it.CanBo)
                  .WithMany(it => it.DanhGiaHoiViens)
                  .HasForeignKey(it => it.IDHoiVien)
                  .HasConstraintName("FK_DanhGiaHoiVien_HoiVien");
            });
            builder.Entity<DanhGiaToChucHoi>(entity => {
                entity.ToTable("DanhGiaToChucHoi", "HV");
                entity.HasKey(it => it.ID);
                entity.HasOne<DiaBanHoatDong>(it => it.DiaBanHoatDong)
                  .WithMany(it => it.DanhGiaToChucHois)
                  .HasForeignKey(it => it.IDDiaBanHoi)
                  .HasConstraintName("FK_DanhGiaToChucHoi_DiaBanHoatDong");
            });
            builder.Entity<DonVi>(entity => {
                entity.ToTable("DonVi", "HV");
                entity.HasKey(it => it.IDDonVi);
            });
            builder.Entity<DoanTheChinhTri_HoiDoan_HoiVien>(tbl =>
            {
                tbl.ToTable("DoanTheChinhTri_HoiDoan_HoiVien", "HV");
                tbl.HasKey(it => new {it.MaDoanTheChinhTri_HoiDoan,it.IDHoiVien });
                tbl.HasOne<CanBo>(it => it.HoiVien)
                  .WithMany(it => it.DoanTheChinhTri_HoiDoan_HoiViens)
                  .HasForeignKey(it => it.IDHoiVien)
                  .HasConstraintName("FK_DoanTheChinhTri_HoiDoan_HoiVien");

                tbl.HasOne<DoanTheChinhTri_HoiDoan>(it => it.DoanTheChinhTri_HoiDoan)
                  .WithMany(it => it.DoanTheChinhTri_HoiDoan_HoiViens)
                  .HasForeignKey(it => it.MaDoanTheChinhTri_HoiDoan)
                  .HasConstraintName("FK_DoanTheChinhTri_HoiDoan_HoiVien_MaDoanTheChinhTri_HoiDoan");

            });

            builder.Entity<HoiVien_ChiHoi>(tbl =>
            {
                tbl.ToTable("HoiVien_ChiHoi", "HV");
                tbl.HasKey(it => new { it.MaChiHoi, it.IDHoiVien });

                tbl.HasOne<CanBo>(it => it.HoiVien)
                  .WithMany(it => it.HoiVienChiHois)
                  .HasForeignKey(it => it.IDHoiVien)
                  .HasConstraintName("FK_HoiVien_ChiHoi_CanBo");

                tbl.HasOne<ChiHoi>(it => it.ChiHoi)
                  .WithMany(it => it.ChiHoiHoiViens)
                  .HasForeignKey(it => it.MaChiHoi)
                  .HasConstraintName("FK_HoiVien_ChiHoi_ChiHoi");

            });

            builder.Entity<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien>(tbl =>
            {
                tbl.ToTable("CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien", "HV");
                tbl.HasKey(it => new { it.Id_CLB_DN_MH_HTX_THT, it.IDHoiVien });
                tbl.HasOne<CanBo>(it => it.HoiVien)
                  .WithMany(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens)
                  .HasForeignKey(it => it.IDHoiVien)
                  .HasConstraintName("FK_CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien");

                tbl.HasOne<CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac>(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac)
                  .WithMany(it => it.CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiViens)
                  .HasForeignKey(it => it.Id_CLB_DN_MH_HTX_THT)
                  .HasConstraintName("FK_CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien");

            });
            builder.Entity<ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien>(tbl =>
            {
                tbl.ToTable("ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien", "HV");
                tbl.HasKey(it => new { it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe, it.IDHoiVien });
                tbl.HasOne<CanBo>(it => it.HoiVien)
                  .WithMany(it => it.ToHoiNganhNghe_ChiHoiNganhNghe_HoiViens)
                  .HasForeignKey(it => it.IDHoiVien)
                  .HasConstraintName("FK_ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien");

                tbl.HasOne<ToHoiNganhNghe_ChiHoiNganhNghe>(it => it.ToHoiNganhNghe_ChiHoiNganhNghe)
                  .WithMany(it => it.ToHoiNganhNghe_ChiHoiNganhNghe_HVs)
                  .HasForeignKey(it => it.Ma_ToHoiNganhNghe_ChiHoiNganhNghe)
                  .HasConstraintName("FK_ToHoiNganhNghe_ChiHoiNganhNghe_ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien");

            });
            builder.Entity<LichSinhHoatChiToHoi>(tbl => {
                tbl.ToTable("LichSinhHoatChiToHoi", "HV");
                tbl.HasKey(it => it.ID);
                tbl.Property(it => it.TenNoiDungSinhHoat).HasMaxLength(500).IsRequired(true);
                tbl.Property(it => it.NoiDungSinhHoat).IsRequired(true);

                tbl.HasOne<DiaBanHoatDong>(it=>it.DiaBanHoatDong)
                .WithMany(it=>it.LichSinhHoatChiToHois)
                .HasForeignKey(it=>it.IDDiaBanHoiVien)
                .HasConstraintName("FK_LichSinhHoatChiToHoi_DiaBanHoatDong");
            });
            builder.Entity<LichSinhHoatChiToHoi_NguoiThamGia>(tbl => {
                tbl.ToTable("LichSinhHoatChiToHoi_NguoiThamGia", "HV");
                tbl.HasKey(it=>it.ID);
                tbl.Property(it => it.MaHoiVien).HasMaxLength(50).IsRequired(true);
                tbl.Property(it => it.TenHoiVien).HasMaxLength(200).IsRequired(true);
                tbl.Property(it => it.ChucVu).HasMaxLength(200).IsRequired(true);
                tbl.HasOne<LichSinhHoatChiToHoi>(it => it.LichSinhHoatChiToHoi)
                .WithMany(it => it.LichSinhHoatChiToHoi_NguoiThamGias)
                .HasForeignKey(it => it.IDLichSinhHoatChiToHoi)
                .HasConstraintName("FK_LichSinhHoatChiToHoi_NguoiThamGia_LichSinhHoatChiToHoi");
            });
            builder.Entity<PhatTrienDang>(tbl => {
                tbl.ToTable("PhatTrienDang", "HV");
                tbl.HasKey(it => it.ID);
                tbl.Property(it => it.TenVietTat).HasMaxLength(500).IsRequired(true);
                tbl.Property(it => it.NoiDung).IsRequired(true);

                tbl.HasOne<DiaBanHoatDong>(it => it.DiaBanHoatDong)
                .WithMany(it => it.PhatTrienDangs)
                .HasForeignKey(it => it.MaDiaBanHoiND)
                .HasConstraintName("FK_PhatTrienDang_DiaBanHoatDong");
            });
            builder.Entity<PhatTrienDang_HoiVien>(tbl => {
                tbl.ToTable("PhatTrienDang_HoiVien", "HV");
                tbl.HasKey(it => new { it.IDPhatTrienDang, it.IDHoiVien });

                tbl.HasOne<PhatTrienDang>(it => it.PhatTrienDang)
                .WithMany(it => it.PhatTrienDang_HoiViens)
                .HasForeignKey(it => it.IDPhatTrienDang)
                .HasConstraintName("FK_PhatTrienDang_PhatTrienDang_HoiVien");

                tbl.HasOne<CanBo>(it => it.CanBo)
               .WithMany(it => it.PhatTrienDang_HoiViens)
               .HasForeignKey(it => it.IDHoiVien)
               .HasConstraintName("FK_CanBo_PhatTrienDang_HoiVien");
            });
            builder.Entity<HoiVienCapThe>(tbl => {
                tbl.ToTable("HoiVienCapThe", "HV");
                tbl.HasKey(it => new { it.ID });

                tbl.HasOne<Dot>(it => it.Dot)
                .WithMany(it => it.HoiVienCapThes)
                .HasForeignKey(it => it.MaDot)
                .HasConstraintName("FK_HoiVienCapThe_Dot");

                tbl.HasOne<CanBo>(it => it.HoiVien)
               .WithMany(it => it.HoiVienCapThes)
               .HasForeignKey(it => it.IDHoiVien)
               .HasConstraintName("FK_HoiVienCapThe_CanBo");
            });
            #endregion Hội Viên
        }

        #region Proc
        [NotMapped]
        public DbSet<CanBoDenTuoiNghiHuuVM> CanBoDenTuoiNghiHuuVMs { get; set; }
        public virtual List<CanBoDenTuoiNghiHuuVM> XetNghiHuu(XetNghiHuuSearchVM search) => CanBoDenTuoiNghiHuuVMs.FromSqlRaw<CanBoDenTuoiNghiHuuVM>(@"EXECUTE dbo.GetDanhSachDenTuoiNghiHuu {0},{1},{2},{3},{4},{5},{6}",
                    search.TuNgay, search.Nam_Tuoi!, search.Nam_Thang!, search.Nu_Tuoi!, search.Nu_Thang!, search.IdCoSo!, search.IdDepartment!)
                .ToList();
        #endregion Proc
    }
}
