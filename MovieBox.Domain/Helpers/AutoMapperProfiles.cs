using AutoMapper;
using MovieBox.Domain.DTOs;
using MovieBox.Domain.Entities;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBox.Domain.Helpers
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();


            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>()
                .ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<MovieCinema, MovieCinemaDTO>()
               .ForMember(x => x.Latitude, dto => dto.MapFrom(prop => prop.Location.Y))
               .ForMember(x => x.Longitude, dto => dto.MapFrom(prop => prop.Location.X));

            CreateMap<MovieCinemaCreationDTO, MovieCinema>()
                .ForMember(x => x.Location, x => x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));

            CreateMap<MovieCreationDTO, Movie>()
               .ForMember(x => x.Poster, options => options.Ignore())
               .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenres))
               .ForMember(x => x.MovieCinemasMovies, options => options.MapFrom(MapMovieCinemasMovies))
               .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors));

            CreateMap<Movie, MovieDTO>()
               .ForMember(x => x.Genres, options => options.MapFrom(MapMoviesGenres))
               .ForMember(x => x.MovieCinemas, options => options.MapFrom(MapMovieCinemasMovies))
               .ForMember(x => x.Actors, options => options.MapFrom(MapMoviesActors));


        }

        private List<ActorMovieDTO> MapMoviesActors(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<ActorMovieDTO>();

            if (movie.MoviesActors != null)
            {
                foreach (var moviesActors in movie.MoviesActors)
                {
                    result.Add(new ActorMovieDTO()
                    {
                        Id = moviesActors.ActorId,
                        Name = moviesActors.Actor.Name,
                        Character = moviesActors.Character,
                        Picture = moviesActors.Actor.Picture,
                        Order = moviesActors.Order
                    });
                }
            }

            return result;
        }

        private List<MovieCinemaDTO> MapMovieCinemasMovies(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<MovieCinemaDTO>();

            if (movie.MovieCinemasMovies != null)
            {
                foreach (var movieCinemaMovies in movie.MovieCinemasMovies)
                {
                    result.Add(new MovieCinemaDTO()
                    {
                        Id = movieCinemaMovies.MovieCinemaId,
                        Name = movieCinemaMovies.MovieCinema.Name,
                        Latitude = movieCinemaMovies.MovieCinema.Location.Y,
                        Longitude = movieCinemaMovies.MovieCinema.Location.X
                    });
                }
            }

            return result;
        }

        private List<GenreDTO> MapMoviesGenres(Movie movie, MovieDTO moviedto)
        {
            var result = new List<GenreDTO>();

            if (movie.MoviesGenres != null)
            {
                foreach (var genre in movie.MoviesGenres)
                {
                    result.Add(new GenreDTO() { Id = genre.GenreId, Name = genre.Genre.Name });
                }
            }

            return result;
        }

        private List<MoviesGenres> MapMoviesGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();

            if (movieCreationDTO.GenresIds == null) { return result; }

            foreach (var id in movieCreationDTO.GenresIds)
            {
                result.Add(new MoviesGenres() { GenreId = id });
            }

            return result;
        }

        private List<MovieCinemaMovies> MapMovieCinemasMovies(MovieCreationDTO movieCreationDTO,
           Movie movie)
        {
            var result = new List<MovieCinemaMovies>();

            if (movieCreationDTO.MovieCinemasIds == null) { return result; }

            foreach (var id in movieCreationDTO.MovieCinemasIds)
            {
                result.Add(new MovieCinemaMovies() { MovieCinemaId = id });
            }

            return result;
        }

        private List<MovieActor> MapMoviesActors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MovieActor>();

            if (movieCreationDTO.Actors == null) { return result; }

            foreach (var actor in movieCreationDTO.Actors)
            {
                result.Add(new MovieActor() { ActorId = actor.Id, Character = actor.Character });
            }

            return result;
        }


    }
}
