using System;
using System.Linq;

namespace MID_BIT247620.Models
{
    public static class DbInitializer
    {
        public static void Seed(RoomDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Look for any room types.
            if (context.RoomTypes_BIT247620.Any())
            {
                return;   // DB has been seeded
            }

            // Seed RoomTypes
            var roomTypes = new RoomType_BIT247620[]
            {
                new RoomType_BIT247620 { Name = "Phòng Trọ Tiêu Chuẩn", Description = "Phòng trọ phổ thông đầy đủ tiện nghi cơ bản, giá cả hợp lý." },
                new RoomType_BIT247620 { Name = "Phòng Trọ Cao Cấp (VIP)", Description = "Phòng diện tích lớn, nội thất hiện đại, điều hòa, tủ lạnh riêng." },
                new RoomType_BIT247620 { Name = "Căn Hộ Studio", Description = "Không gian khép kín thông minh, thích hợp cho người đi làm hoặc gia đình nhỏ." }
            };

            foreach (var t in roomTypes)
            {
                context.RoomTypes_BIT247620.Add(t);
            }
            context.SaveChanges();

            // Seed Rooms
            var rooms = new Room_BIT247620[]
            {
                new Room_BIT247620 { Name = "Phòng 101", Price = 1500000, Area = 18.5, IsAvailable = true, Description = "Phòng tầng trệt, thoáng mát, vệ sinh khép kín.", RoomTypeId = roomTypes[0].Id },
                new Room_BIT247620 { Name = "Phòng 102", Price = 1800000, Area = 22.0, IsAvailable = true, Description = "Phòng tầng 1, ban công rộng, có gác lửng.", RoomTypeId = roomTypes[0].Id },
                new Room_BIT247620 { Name = "Phòng 201 (VIP)", Price = 3200000, Area = 30.0, IsAvailable = true, Description = "Phòng VIP có sẵn điều hòa, nóng lạnh, giường tủ gỗ.", RoomTypeId = roomTypes[1].Id },
                new Room_BIT247620 { Name = "Phòng 202 (VIP)", Price = 3500000, Area = 35.0, IsAvailable = false, Description = "Phòng VIP căn góc, 2 cửa sổ lớn, view thoáng.", RoomTypeId = roomTypes[1].Id },
                new Room_BIT247620 { Name = "Phòng 301 Studio", Price = 4500000, Area = 45.0, IsAvailable = true, Description = "Căn hộ Studio cao cấp tầng thượng, nội thất sang trọng.", RoomTypeId = roomTypes[2].Id },
                new Room_BIT247620 { Name = "Phòng 302 Chưa Có Ảnh", Price = 2000000, Area = 20.0, IsAvailable = true, Description = "Phòng tiêu chuẩn trống, đang dọn dẹp, chưa cập nhật ảnh.", RoomTypeId = roomTypes[0].Id }
            };

            foreach (var r in rooms)
            {
                context.Rooms_BIT247620.Add(r);
            }
            context.SaveChanges();

            // Seed RoomImages
            var images = new RoomImage_BIT247620[]
            {
                // Images for Room 101
                new RoomImage_BIT247620 { ImageUrl = "https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?auto=format&fit=crop&w=600&q=80", IsThumbnail = true, RoomId = rooms[0].Id },
                new RoomImage_BIT247620 { ImageUrl = "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?auto=format&fit=crop&w=600&q=80", IsThumbnail = false, RoomId = rooms[0].Id },

                // Images for Room 102
                new RoomImage_BIT247620 { ImageUrl = "https://images.unsplash.com/photo-1598928506311-c55ded91a20c?auto=format&fit=crop&w=600&q=80", IsThumbnail = true, RoomId = rooms[1].Id },

                // Images for Room 201 (VIP)
                new RoomImage_BIT247620 { ImageUrl = "https://images.unsplash.com/photo-1505691938895-1758d7feb511?auto=format&fit=crop&w=600&q=80", IsThumbnail = true, RoomId = rooms[2].Id },
                new RoomImage_BIT247620 { ImageUrl = "https://images.unsplash.com/photo-1540518614846-7eded433c457?auto=format&fit=crop&w=600&q=80", IsThumbnail = false, RoomId = rooms[2].Id },

                // Images for Room 202 (VIP)
                new RoomImage_BIT247620 { ImageUrl = "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?auto=format&fit=crop&w=600&q=80", IsThumbnail = true, RoomId = rooms[3].Id },

                // Images for Room 301 Studio
                new RoomImage_BIT247620 { ImageUrl = "https://images.unsplash.com/photo-1536376072261-38c75010e6c9?auto=format&fit=crop&w=600&q=80", IsThumbnail = true, RoomId = rooms[4].Id },
                new RoomImage_BIT247620 { ImageUrl = "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?auto=format&fit=crop&w=600&q=80", IsThumbnail = false, RoomId = rooms[4].Id }
                // Note: Room 302 (rooms[5]) has no images to verify error handling for "phòng chưa có ảnh"
            };

            foreach (var img in images)
            {
                context.RoomImages_BIT247620.Add(img);
            }
            context.SaveChanges();
        }
    }
}
