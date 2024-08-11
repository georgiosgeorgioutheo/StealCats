using Core.DTOs;
using Core.Entities;

namespace Application.Mappings
{
    public static class CatMappings
    {
        public static CatResponseDto ToCatResponse(CatEntity cat, string baseUrl)
        {
            return new CatResponseDto
            {
                Id = cat.Id,
                CatId = cat.CatId,
                Width = cat.Width,
                Height = cat.Height,
                Created = cat.Created,
                ImageUrl = $"{baseUrl}/api/cats/{cat.Id}/image",
                Tags = cat.CatTags.Select(ct => ct.Tag.Name).ToList()
            };
        }
        public static async Task<CatEntity> ToCatEntityAsync(this CatApiResponse catApiResponse, HttpClient httpClient)
        {

            var catEntity = new CatEntity
            {
                CatId = catApiResponse.Id,
                Width = catApiResponse.Width,
                Height = catApiResponse.Height,
                Image = await httpClient.GetByteArrayAsync(catApiResponse.Url),
                Created = DateTime.UtcNow,
                CatTags = new List<CatTagEntity>()
            };

            AddTagsToCatEntity(catApiResponse, catEntity);

            return catEntity;
        }

        private static void AddTagsToCatEntity(CatApiResponse catApiResponse, CatEntity catEntity)
        {
            if (catApiResponse.Breeds != null)
            {
                foreach (var breed in catApiResponse.Breeds)
                {
                    var tags = ExtractTagsFromBreed(breed);

                    AddTagEntitiesToCatEntity(tags, catEntity);
                }
            }
        }

        private static List<string> ExtractTagsFromBreed(Breed breed)
        {
            if (breed?.Temperament == null)
            {
                return new List<string>();
            }

            return breed.Temperament
                        .Split(',')
                        .Select(t => t.Trim())
                        .ToList();
        }
        private static  void AddTagEntitiesToCatEntity(List<string> tags, CatEntity catEntity)
        {
            foreach (var tag in tags)
            {
                var tagEntity = new TagEntity
                {
                    Name = tag,
                    Created = DateTime.UtcNow
                };
                catEntity.CatTags.Add(new CatTagEntity
                {
                    Cat = catEntity,
                    Tag = tagEntity
                });
            }
        }

    }

}