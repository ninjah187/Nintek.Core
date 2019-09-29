using Nintek.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nintek.Core.Tests
{
    public class TrackingListTests
    {
        readonly Data[] _sampleData = new[]
        {
            new Data(),
            new Data(),
            new Data()
        };

        [Fact]
        public void ParameterlessConstructor_CreatesEmptyList()
        {
            var list = new TrackingList<Data>();

            Assert.Empty(list);
            Assert.Empty(list.Added);
            Assert.Empty(list.Removed);
        }

        [Fact]
        public void ConstructorWithData_CreatesListFilledWithTheData()
        {
            var list = new TrackingList<Data>(_sampleData);

            Assert.Equal(_sampleData, list);
            Assert.Empty(list.Added);
            Assert.Empty(list.Removed);
        }

        [Fact]
        public void AddingItem_ToEmptyList_IsSuccessful()
        {
            var data = new Data();
            var dataArray = new[] { data };
            var list = new TrackingList<Data>();

            list.Add(data);

            Assert.Equal(dataArray, list);
            Assert.Equal(dataArray, list.Added);
            Assert.Empty(list.Removed);
        }

        [Fact]
        public void AddingItem_ToListWithData_IsSuccessful()
        {
            var data = new Data();
            var dataArray = new[] { data };
            var list = new TrackingList<Data>(_sampleData);

            list.Add(data);

            Assert.Equal(_sampleData.Concat(dataArray), list);
            Assert.Equal(dataArray, list.Added);
            Assert.Empty(list.Removed);
        }

        [Fact]
        public void RemovingItem_WhichWasAlreadyInList_IsSuccessful()
        {
            var list = new TrackingList<Data>(_sampleData);

            list.Remove(_sampleData.First());

            Assert.Equal(_sampleData.Skip(1), list);
            Assert.Empty(list.Added);
            Assert.Equal(_sampleData.Take(1), list.Removed);
        }

        [Fact]
        public void AddingAndThenRemovingTheSameItem_FromEmptyList_IsSuccessful()
        {
            var data = new Data();
            var dataArray = new[] { data };
            var list = new TrackingList<Data>();

            list.Add(data);
            list.Remove(data);

            Assert.Empty(list);
            Assert.Empty(list.Added);
            Assert.Empty(list.Removed);
        }

        [Fact]
        public void AddingAndThenRemovingTheSameItem_FromListWithData_IsSuccessful()
        {
            var data = new Data();
            var dataArray = new[] { data };
            var list = new TrackingList<Data>(_sampleData);

            list.Add(data);
            list.Remove(data);

            Assert.Equal(_sampleData, list);
            Assert.Empty(list.Added);
            Assert.Empty(list.Removed);
        }
    }
}
