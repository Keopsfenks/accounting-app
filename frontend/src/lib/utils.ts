import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"
import {AuthService} from "@/services/AuthService";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}


