import axios from "axios";
import { env } from "../../../env/env.dev";
import { endpoints } from "../config/apiConfig";
import { Post } from "../../models/Post";

export const apiFetchSuggestedPostsRequest = async (userId: number): Promise<Post[] | undefined> => {
  try {
    const response = await axios.get(`${env.baseUrl}${endpoints.getPosts(userId)}`);
    // console.log(response);
    return response.data as Post[];
  } catch (error) {
    console.error('Error fetching comments:', error);
    return undefined;
  }
};