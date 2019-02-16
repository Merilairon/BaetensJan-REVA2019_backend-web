using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Xunit;
using Moq;


namespace ApplicationCore.Tests.Services
{
    public class CategoryManagerTest
    {
        private List<Category> _pickedCategories;
        private List<Category> _unpickedCategories;

        private List<Category> _categories;
        private List<Question> _questions;
        private List<Assignment> _assignments;

        private Mock<IExhibitorRepository> _exhibitorRepo;
        private Mock<ICategoryRepository> _categoryRepo;
        private Mock<IQuestionRepository> _questionRepo;

        public CategoryManagerTest()
        {
            #region 9 Categories

            _categories = new List<Category>
            {
                new Category
                {
                    Id = 1
                },
                new Category
                {
                    Id = 2
                },
                new Category
                {
                    Id = 3
                },
                new Category
                {
                    Id = 4
                },
                new Category
                {
                    Id = 5
                },
                new Category
                {
                    Id = 6
                },
                new Category
                {
                    Id = 7
                },
                new Category
                {
                    Id = 8
                },
                new Category
                {
                    Id = 9
                }
            };

            #endregion

            #region Questions

            _questions = new List<Question>
            {
                new Question
                {
                    Id = 1,
                    CategoryExhibitor = new CategoryExhibitor
                    {
                        CategoryId = 1
                    }
                },
                new Question
                {
                    Id = 2,
                    CategoryExhibitor = new CategoryExhibitor
                    {
                        CategoryId = 2
                    }
                },
                new Question
                {
                    Id = 3,
                    CategoryExhibitor = new CategoryExhibitor
                    {
                        CategoryId = 3
                    }
                },
                new Question
                {
                    Id = 4,
                    CategoryExhibitor = new CategoryExhibitor
                    {
                        CategoryId = 4
                    }
                },
                new Question
                {
                    Id = 5,
                    CategoryExhibitor = new CategoryExhibitor
                    {
                        CategoryId = 5
                    }
                },
                new Question
                {
                    Id = 6,
                    CategoryExhibitor = new CategoryExhibitor
                    {
                        CategoryId = 6
                    }
                },
                new Question
                {
                    Id = 7,
                    CategoryExhibitor = new CategoryExhibitor
                    {
                        CategoryId = 7
                    }
                },
                new Question
                {
                    Id = 8,
                    CategoryExhibitor = new CategoryExhibitor
                    {
                        CategoryId = 8
                    }
                },
                new Question
                {
                    Id = 9,
                    CategoryExhibitor = new CategoryExhibitor
                    {
                        CategoryId = 9
                    }
                }
            };

            #endregion

            #region 7 Assignments

            _assignments = new List<Assignment>
            {
                new Assignment
                {
                    Question = _questions[0]
                },
                new Assignment
                {
                    Question = _questions[1]
                },
                new Assignment
                {
                    Question = _questions[2]
                },
                new Assignment
                {
                    Question = _questions[3]
                },
                new Assignment
                {
                    Question = _questions[4]
                },
                new Assignment
                {
                    Question = _questions[5]
                },
                new Assignment
                {
                    Question = _questions[6]
                },
            };

            #endregion

            _categoryRepo = new Mock<ICategoryRepository>();
            _categoryRepo.Setup(t => t.All()).Returns(Task.FromResult<IEnumerable<Category>>(_categories));

            _exhibitorRepo = new Mock<IExhibitorRepository>();

            _questionRepo = new Mock<IQuestionRepository>();
            _questionRepo.Setup(t => t.GetAll()).Returns(Task.FromResult(_questions));
        }

        [Fact]
        public void GetUnpickedCategoriesNormalTourTest()
        {
            var categories = new CategoryManager(_categoryRepo.Object, _exhibitorRepo.Object, _questionRepo.Object)
                .GetUnpickedCategoriesNormalTour(_assignments, _categories);

            var id = 8;
            // _assigments contains question with id 1 -> 7
            // every Question has a Category, with QuestionId == CategoryId
            // So: Categories 1 -> 7 were picked, Category 8 -> 9 not.
            foreach (var category in categories)
            {
                Assert.Equal(id, category.Id);
                id++;
            }
        }

        [Fact]
        public void GetUnpickedCategoriesExtraTourTest()
        {
            _questions.Add(new Question // this is a question that also (just as Question2) belongs to Category 2
            {
                Id = 10,
                CategoryExhibitor = new CategoryExhibitor
                {
                    CategoryId = 2
                }
            });

            var categories = new CategoryManager(_categoryRepo.Object, _exhibitorRepo.Object, _questionRepo.Object)
                .GetUnpickedCategoriesExtraRound(_assignments, _categories, _questions).ToList();

            // Category with id 2 has 2 questions: one that is already answered/picked (question2) by the Group
            // and one unanswered/unpicked (question10, with Category 2)
            var ids = new[] {2, 8, 9};

            foreach (var category in categories)
            {
                Assert.True(ids.Contains(category.Id));
            }

            // check if we have only 3 categories.
            Assert.True(categories.Count == ids.Length);
            
            // sort categories on id (asc)
            categories.Sort((cat1, cat2) => cat1.Id.CompareTo(cat2.Id));
            
            // check if category with lowest id == 2 (the one we added in this Test method)
            Assert.Equal(categories[0].Id, ids[0]);
        }

        [Theory]
        [InlineData(false, -1)]
        [InlineData(true, 2)]
        public void GetUnpickedCategoriesTest(bool extraRound, int exhibitorId)
        {
            _exhibitorRepo.Setup(t => t.GetById(It.IsAny<int>())).Returns(Task.FromResult(new Exhibitor
            {
                Categories = new List<CategoryExhibitor>
                {
                    new CategoryExhibitor
                    {
                        Category = _categories[0]
                    }
                }
            }));
        }
    }
}