﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using System.Data.Entity;


namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private VidlyContext _context;

        public MoviesController()
        {
            _context = new VidlyContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Movies
        public ActionResult Index()
        {
            var movies = _context.Movies.Include(c => c.Genre).ToList();
            return View(movies);
        }

        [HttpPost]
        public ActionResult Save(Movie movie)
        {
            if(movie.Movieid == 0)
            {
                _context.Movies.Add(movie);
            }
            else
            {
                var movieInDb = _context.Movies.Single(c => c.Movieid == movie.Movieid);
                movieInDb.Name = movie.Name;
                movieInDb.Classification = movie.Classification;
                movieInDb.GenreId = movie.GenreId;
                movieInDb.Genre = movie.Genre;
            }
            _context.SaveChanges();
            return View();
        }



        [Route("movies/details/{id}")]
        public ActionResult Details(int id)
        {
            var movies = _context.Movies.Include(c => c.Genre).SingleOrDefault(c => c.Movieid == id);

            if (movies == null)
            {
                return HttpNotFound();
            }
            return View(movies);
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Movieid == id);
            var viewModel = new MoviesFormViewModel
            {
                Movie = movie,
                Genres = _context.Genres.ToList()
            };
            return View("MoviesForm", viewModel);
        }
    }
}