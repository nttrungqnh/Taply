using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Taply.Models;

namespace Taply.Services;

public static class SeedData
{
    public static async Task Initialize(TaplyDbContext db)
    {
        if (await db.Users.AnyAsync()) return;

        var user = new User { Email = "admin@taply.vn", DisplayName = "Taply Admin" };
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, "Taply123!");
        var place = new Place
        {
            User = user,
            Name = "Afflactre",
            Slug = "afflactre",
            Address = "À La Carte Đà Nẵng",
            Phone = "09xx xxx xxx",
            Zalo = "09xx xxx xxx",
            Description = "Thông tin dành cho khách lưu trú tại Afflactre."
        };
        var page = new Page
        {
            Place = place,
            Name = "À La Carte B3605",
            Slug = "afflactre-b3605",
            Subtitle = "Thông tin nhanh dành cho khách lưu trú",
            SortOrder = 0
        };
        page.Sections.AddRange([
            new PageSection { Type = "wifi", Title = "Wi-Fi", Subtitle = "Thông tin kết nối", Icon = "bi-wifi", Content = "{\"ssid\":\"B3605 5G\",\"password\":\"afflactre2026\"}", SortOrder = 0 },
            new PageSection { Type = "door_access", Title = "Mật khẩu cửa", Icon = "bi-lock", Content = "{\"code\":\"2580#\",\"note\":\"Bấm mã rồi nhấn #\",\"isSensitive\":true}", SortOrder = 1 },
            new PageSection { Type = "instruction", Title = "Máy chiếu", Icon = "bi-projector", Content = "{\"steps\":[\"Bật công tắc cạnh giường\",\"Nhấn nút Power\",\"Chọn HDMI 1\",\"Kết nối laptop hoặc điện thoại\"]}", SortOrder = 2 },
            new PageSection { Type = "text", Title = "Điều hòa", Icon = "bi-snow", Content = "Remote đặt trên bàn cạnh giường.", SortOrder = 3 },
            new PageSection { Type = "contact", Title = "Liên hệ", Icon = "bi-telephone", Content = "{\"phone\":\"09xx xxx xxx\",\"zalo\":\"09xx xxx xxx\",\"note\":\"Hỗ trợ 24/7\"}", SortOrder = 4 },
            new PageSection { Type = "checkout", Title = "Check-out", Icon = "bi-door-open", Content = "{\"time\":\"12:00\",\"note\":\"Vui lòng tắt điều hòa và khóa cửa khi rời đi.\"}", SortOrder = 5 },
            new PageSection { Type = "rules", Title = "Lưu ý", Icon = "bi-card-checklist", Content = "{\"items\":[\"Không hút thuốc trong phòng\",\"Giữ yên tĩnh sau 22:00\",\"Liên hệ chủ nhà nếu cần hỗ trợ\"]}", SortOrder = 6 },
            new PageSection { Type = "nearby", Title = "Ăn gì gần đây", Icon = "bi-geo-alt", Content = "{\"items\":[\"Lẩu gà lá é Tao Ngộ · 1.2 km\",\"Tiệm cà phê ABC · 800 m\"]}", SortOrder = 7 }
        ]);

        db.Users.Add(user);
        db.Places.Add(place);
        await db.SaveChangesAsync();
    }
}
