import { ReactNode, useState } from "react";
import NavBar from "../components/NavBar";


interface Props {
    children: ReactNode
}

export function QuizLayout({children}: Props) {
    return(
        <div className="bg-color">
            <NavBar/>
            <div className="mt-5">
                {children}
            </div>
        </div>
    )
}
