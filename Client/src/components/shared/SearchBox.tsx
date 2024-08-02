import { useState } from "react";

interface Props {
    onSearch : (value:string) => void
}

export function SearchBox({onSearch}:Props) { 
    const [value, setValue] = useState("");
    return (<div className="search-box">
    <input type="text" value={value} className="search-text-input"
        onInput={(e) => setValue(e.currentTarget.value)}/>
    <button onClick={() => onSearch(value)} className="btn search-btn active-btn">S</button>
    </div>)
}