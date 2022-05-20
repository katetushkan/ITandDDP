import React from 'react'
import Hero from '../components/AuthPageComponents/Hero'

import '../Auth.css';

export default function Auth() {
    return (
        <div className='auth-container'>
            <nav className="navbar">
                <a className="logo" href="https://github.com/Andrushens/ITandDDP-1">About Us</a>
            </nav>
            <Hero />
            <section className="reviews">
                <ul>
                    <li>
                        <blockquote>"Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor
                            invidunt ut labore et dolore"</blockquote>
                        <cite>- John Doe</cite>
                    </li>
                    <li>
                        <blockquote>"Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor
                            invidunt ut labore et dolore"</blockquote>
                        <cite>- Dohn Joe</cite>
                    </li>
                    <li>
                        <blockquote>"Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor
                            invidunt ut labore et dolore"</blockquote>
                        <cite>- HnoJ Eod</cite>
                    </li>
                </ul>
            </section>
        </div>
    )
}
